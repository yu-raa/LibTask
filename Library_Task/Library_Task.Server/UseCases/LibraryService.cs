using AutoMapper;
using Library_Task.Server.DTO;
using Library_Task.Server.Policies;
using Library_Task.Server.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Modsen_Library_Test_Task.Entities;

namespace Library_Task.Server.UseCases
{
    public interface IReadServiceAsync<T> where T : BusinessEntity
    {
        Task<KeyValuePair<List<T>, int>> GetAllAsync(int pageIndex = 1, int pageSize = 20, string? search = null, string? authorToFilter = null, string? genreToFilter = null);
        Task<object> GetByIdAsync(string id);
    }

    public interface IServiceAsync<T> : IReadServiceAsync<T> where T : BusinessEntity

    {
        Task<IActionResult> AddAsync(T item);
        Task<bool> DeleteAsync(string id);
        Task<IActionResult> AlterAsync(T item);
        Task<bool> GiveReaderBook(string readerId, string bookId);
        Task<bool> NotifyAboutReturningBook(string readerId, string bookId);
        Task<object> GetAllAuthorBooks(string authorId, int pageIndex = 1, int pageSize = 20);

    }

    public class LibraryService<T> : IServiceAsync<T> where T : class, BusinessEntity
    {
        private readonly IUnitOfWork<T> _unitOfWork;
        private readonly IUnitOfWork<Reader> _unitOfWork2;
        private readonly IMapper _mapper;
        private readonly Type _type;

        public LibraryService(IUnitOfWork<T> unitOfWork, IMapper mapper, IUnitOfWork<Reader> unitOfWork2)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork2 = unitOfWork2;
            _mapper = mapper;
            _type = typeof(T) == typeof(Book) ? typeof(DatabaseBook) : typeof(DatabaseAuthor);
        }

        public async Task<object?> GetBookByIsbn(string isbn)
        {
            var book = ( await _unitOfWork.Repository().GetAll()).Select(obj => (DatabaseBook)obj).FirstOrDefault(book => book.ISBN == isbn);
            return book is null? null : _mapper.Map<T>(book);
        }

        public async Task<bool> AddBookImage(string id, string uri)
        {
            DatabaseBook? databaseBook = await _unitOfWork.Repository().GetById(id) as DatabaseBook;

            if (databaseBook is null)
            {
                return false;
            }

            try
            {
                var res = Convert.FromBase64String(uri);
                databaseBook.BookImage = uri;
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> GiveReaderBook(string readerId, string bookId)
        {
            var book = (await (_unitOfWork.Repository().GetAll())).Select(obj => (DatabaseBook)obj).FirstOrDefault(book => book.Id == bookId);
            var user = (await (_unitOfWork2.Repository().GetAll())).Select(obj => (DatabaseUser)obj).FirstOrDefault(reader => reader.Id == readerId);

            if (book is null || user is null) return false;

            book.UserId = user.Id;

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NotifyAboutReturningBook(string readerId, string bookId)
        {
            var books = (await _unitOfWork.Repository().GetAll()).Where(book => book.GetType().GetProperty("UserId").GetValue(book) == readerId);
            return books?.SingleOrDefault(book => book.GetType().GetProperty("Id").GetValue(book) == bookId && (DateTime)book.GetType().GetProperty("LastToReturn").GetValue(book) <= DateTime.UtcNow) is not null;
        }

        public async Task<object> GetAllAuthorBooks(string authorId, int pageIndex = 1, int pageSize = 20)
        {
            var arr = (await _unitOfWork.Repository().GetAll()).Where(book => book.GetType().GetProperty("AuthorId").GetValue(book) == authorId)
            .OrderBy(book => book.GetType().GetProperty("Id").GetValue(book))
            .Skip((pageIndex - 1) * pageSize).AsEnumerable()
            .ToArray();
            arr = arr?[..(pageSize > arr.Length ? arr.Length : pageSize)];
            var books = new List<T>();
            foreach (var re in arr)
            {
                books.Add(_mapper.Map<T>(re));
            }

            return books;
        }

        async Task<IActionResult> IServiceAsync<T>.AddAsync(T dto)
        {
            if (typeof(T) == typeof(Author) || new Validator().Validate(dto).IsValid)
            {
                await _unitOfWork.Repository().Add(_mapper.Map(dto, typeof(T), _type));
                await _unitOfWork.SaveChangesAsync();
                return new OkResult();
            }

            return new BadRequestResult();
        }

        async Task<IActionResult> IServiceAsync<T>.AlterAsync(T dto)
        {
            if (new Validator().Validate(dto).IsValid)
            {
                var newItemState = _mapper.Map(dto, typeof(T), _type);
                var _items = (await _unitOfWork.Repository().GetAll()).Select(obj => (DatabaseBook)obj).ToList();
                var oldItemState = _items.First(item => item.Id == newItemState.GetType().GetProperty("Id").GetValue(newItemState));
                if (oldItemState is not null)
                {
                    foreach (var prop in newItemState.GetType().GetProperties())
                    {
                        prop.SetValue(oldItemState, prop.GetValue(newItemState));
                    }
                    await _unitOfWork.Repository().Alter(oldItemState);
                }
                
                return new OkResult();
            }

            return new BadRequestResult();
        }

        async Task<bool> IServiceAsync<T>.DeleteAsync(string id)
        {
            T? item = null;
            var res = await _unitOfWork.Repository().GetById(id);

            item = _mapper.Map(res, _type, typeof(T)) as T;

            if (item is not null)
            {

                await _unitOfWork.Repository().Remove(id);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        async Task<KeyValuePair<List<T>, int>> IReadServiceAsync<T>.GetAllAsync(int pageIndex = 1, int pageSize = 20, string? search = null, string? authorToFilter = null, string? genreToFilter = null)
        {
            var res = await _unitOfWork.Repository().GetAll();

            if (typeof(T) == typeof(Author))
            {
                return new(res.Select(obj => _mapper.Map<T>((DatabaseAuthor)obj)).ToList(), res.Count());
            }

            IQueryable<T> queriable = res.Select(obj => _mapper.Map<T>((DatabaseBook)obj)).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                queriable = queriable.AsEnumerable().Where(item => typeof(T).GetProperty("Title").GetValue(item).ToString().Contains(search)).AsQueryable();
            }

            if (!string.IsNullOrEmpty(authorToFilter))
            {
                queriable = queriable.AsEnumerable().Where(item => typeof(T).GetProperty("AuthorId").GetValue(item).ToString() == authorToFilter).AsQueryable();
            }

            if (!string.IsNullOrEmpty(genreToFilter))
            {
                queriable = queriable.AsEnumerable().Where(item => typeof(T).GetProperty("Genre").GetValue(item).ToString() == genreToFilter).AsQueryable();
            }

            if (queriable is null || queriable.Count() == 0)
                return new(new List<T>(), new int());
            else
            {
                var arr = queriable
            .OrderBy(it => it.Id)
            .Skip((pageIndex - 1) * pageSize).AsEnumerable().
            Select(it => (object)it)
            .ToArray();
                arr = arr[..(pageSize > arr.Length ? arr.Length : pageSize)];
                return new(arr.Select(obj => (T)obj).ToList(), res.Count());
            }
        }

        async Task<object> IReadServiceAsync<T>.GetByIdAsync(string id)
        {
            var res = await _unitOfWork.Repository().GetById(id);
            return _mapper.Map(res, typeof(T), _type);
        }
    }
}
