using Library_Task.Server.DTO;
using Library_Task.Server.Frameworks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Modsen_Library_Test_Task.Entities;
namespace Library_Task.Server.UnitOfWork
{
    public interface IUnitOfWork<T> where T : class, BusinessEntity
    {
        Task SaveChangesAsync();
        IRepository Repository();
    }

    public class UnitOfWork<T> : IUnitOfWork<T> where T : class, BusinessEntity
    {
        private readonly LibraryContext _dbContext;

        public UnitOfWork(LibraryContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public IRepository Repository()
        {
            if (typeof(T) == typeof(Book))
                return new Repository<DatabaseBook>(_dbContext);
            else if (typeof(T) == typeof(Reader))
            {
                return new Repository<DatabaseUser>(_dbContext);
            }
            else return new Repository<DatabaseAuthor>(_dbContext);
        }
    }

    public interface IRepository
    {
        public Task<object?> GetById(string id);
        public Task<List<object>> GetAll();
        public Task Add(object item);
        public Task Remove(object item);
        public Task<EntityEntry> Alter(object newItemState);
    }

    public class Repository<T> : IRepository where T : class, DatabaseItem
    {
        internal LibraryContext _context;
        internal DbSet<T> _items;

        public Repository(LibraryContext dbContext)
        {
            _context = dbContext;
            _items = _context.Set<T>();
        }

        public async Task<object?> GetById(string id)
        {
            return await _items.FirstOrDefaultAsync(item => item.Id == id);
        }
        public async Task<List<object>> GetAll()
        {
            return _items.Select(it => (object)it).ToList();
        }
        public async Task Add(object item)
        {
            var it = (T)item;
            await _items.AddAsync(it);
        }
        public async Task Remove(object item)
        {
            _items?.Remove(await GetById(item.ToString()) as T);
        }

        public async Task<EntityEntry> Alter(object oldItemState)
        {
            return _items.Update(oldItemState as T);
        }
    }
}
