using AutoMapper;
using Library_Task.Server.DTO;
using Library_Task.Server.Frameworks;
using Library_Task.Server.UnitOfWork;
using Library_Task.Server.UseCases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modsen_Library_Test_Task.Entities;
using Moq;

namespace ServerTests
{
    public class UnitTestsForBookUseCases
    {
        private readonly LibraryContext _dbContext;
        private readonly IMapper _mapper;

        public UnitTestsForBookUseCases() 
        {
            var options = new DbContextOptionsBuilder<LibraryContext>().UseInMemoryDatabase("TestDatabase").ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _dbContext = new LibraryContext(options);
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(mapperConfig);
        }

        [Fact]
        public async Task GetBookByEmptyISBN_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).GetBookByIsbn("");
            Assert.Null(res);
        }

        [Fact]
        public async Task GetBookByWrongISBN_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).GetBookByIsbn("qwhs");
            Assert.Null(res);
        }

        [Fact]
        public async Task GetBookByRightISBN_ReturnsRightBook()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var timenow = DateTime.Now;

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = timenow, LastToReturn = timenow + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var book = new Book("1", "1234567890123", "dhs", "hello", "cdhjdch", timenow, timenow + new TimeSpan(72, 12, 30), null);

            book.AuthorId = "1";
            book.UserId = "1";

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).GetBookByIsbn("1234567890123");
            Assert.Equivalent(book, res);
        }

        [Fact]
        public async Task AddImageToBookWithWrongId_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var imageArray = File.ReadAllBytes("C:\\Users\\User\\source\\repos\\Modsen_Library_Test_Task\\Library_Task\\library_task.client\\public\\favicon.ico");

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).AddBookImage("3", Convert.ToBase64String(imageArray));
            Assert.False(res);
        }

        [Fact]
        public async Task AddImageToBookWithWrongBase64String_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var imageArray = File.ReadAllBytes("C:\\Users\\User\\source\\repos\\Modsen_Library_Test_Task\\Library_Task\\library_task.client\\public\\favicon.ico");

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).AddBookImage("1", Convert.ToBase64String(imageArray).Substring(2));
            Assert.False(res);
        }

        [Fact]
        public async Task AddImageToBookWithRightParams_ReturnsTrue()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var imageArray = File.ReadAllBytes("C:\\Users\\User\\source\\repos\\Modsen_Library_Test_Task\\Library_Task\\library_task.client\\public\\favicon.ico");

            var service = new LibraryService<Book>(unitOfWork, _mapper, null);

            var res = await service.AddBookImage("1", Convert.ToBase64String(imageArray));
            Assert.True(res);
        }

        [Fact]
        public async Task NotifyAboutReturningBooksWithWrongReaderId_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).NotifyAboutReturningBook("3jh", "1");
            Assert.False(res);
        }

        [Fact]
        public async Task NotifyAboutReturningBooksWithWrongBookId_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).NotifyAboutReturningBook("1", "3");
            Assert.False(res);
        }

        [Fact]
        public async Task NotifyAboutReturningBooksWithBookEarlyToReturn_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, null).NotifyAboutReturningBook("2", "1");
            Assert.True(res);
        }

        [Fact]
        public async Task GiveReaderBookWithWrongUserId_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var secondUnitOfWork = new UnitOfWork<Reader>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, secondUnitOfWork).GiveReaderBook("3bhx", "1");
            Assert.False(res);
        }

        [Fact]
        public async Task GiveReaderBookWithWrongBookId_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var secondUnitOfWork = new UnitOfWork<Reader>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, secondUnitOfWork).GiveReaderBook("2", "5");
            Assert.False(res);
        }

        [Fact]
        public async Task GiveReaderBookWithRightIds_ReturnsTrue()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var secondUnitOfWork = new UnitOfWork<Reader>(_dbContext);

            var res = await new LibraryService<Book>(unitOfWork, _mapper, secondUnitOfWork).GiveReaderBook("2", "2");
            Assert.True(res);
        }

        [Fact]
        public async Task GetAllAuthorBooksWithWrongId_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;

            var res = await service.GetAllAuthorBooks("43");
            Assert.Empty(res as List<Book>);
        }

        [Fact]
        public async Task GetAllAuthorBooksWithRightId_ReturnsList()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;

            var res = await service.GetAllAuthorBooks("2");
            Assert.NotEmpty(res as List<Book>);
        }

        [Fact]
        public async Task AddBookWithWrongISBN_ReturnsBadRequestResult()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;
            var book = new Book("4", "123467890123", "dhs", "hello", "cdhjdch", timenow, timenow + new TimeSpan(72, 12, 30), null);
            book.AuthorId = "1";
            book.UserId = "1";
            var res = await service.AddAsync(book);
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async Task AlterBookWithRightProperties_ReturnsOkResult()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;
            var book = new Book("1", "1234657890123", "dhs", "hello", "cdhjdch", timenow, timenow + new TimeSpan(72, 12, 30), null);
            book.AuthorId = "1";
            book.UserId = "1";
           
            var res = await service.AlterAsync(book);
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task AlterBookWithWrongISBN_ReturnsBadRequestResult()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            var timenow = DateTime.Now;

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = timenow, LastToReturn = timenow + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var book = new Book("1", "12890123", "dhs", "hello", "cdhjdch", timenow, timenow + new TimeSpan(72, 12, 30), null);
            book.AuthorId = "1";
            book.UserId = "1";
            var res = await service.AlterAsync(book);
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async Task AddBookWithRightProperties_ReturnsOkResult()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;
            var book = new Book("4", "1234657890123", "dhs", "hello", "cdhjdch", timenow, timenow + new TimeSpan(72, 12, 30), null);
            book.AuthorId = "1";
            book.UserId = "1";
            book.BookImage = Convert.ToBase64String(File.ReadAllBytes("C:\\Users\\User\\source\\repos\\Modsen_Library_Test_Task\\Library_Task\\library_task.client\\public\\favicon.ico"));
            var res = await service.AddAsync(book);
            Assert.IsType<OkResult>(res);
        }

        [Fact]
        public async Task DeleteBookWithRightId_ReturnsTrue()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.DeleteAsync("2");
            Assert.True(res);
        }

        [Fact]
        public async Task DeleteBookWithNonExistentId_ReturnsFalse()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.DeleteAsync("324r");
            Assert.False(res);
        }

        [Fact]
        public async Task GetBookByIdWithNonExistentId_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetByIdAsync("324r");
            Assert.Null(res);
        }

        [Fact]
        public async Task GetBookByIdWithRightId_ReturnsABook()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;

            var res = await service.GetByIdAsync("1");
            Assert.NotNull(res);
        }

        [Fact]
        public async Task GetAllBooksDefaultParams_ReturnsTwo()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync();
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 2);
        }

        [Fact]
        public async Task GetAllBooksPageOnePageSizeOneOtherParamsDefault_ReturnsOne()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(1, 1);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 1);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Value, 2);
        }

        [Fact]
        public async Task GetAllBooksPageTwoOtherParamsDefault_ReturnsEmpty()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(2);
            Assert.Empty(res.Key as List<Book>);
        }

        [Fact]
        public async Task GetAllBooksSearchIsInBothOtherParamsDefault_ReturnsTwo()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello2", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(search: "hello");
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 2);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Value, 2);
        }

        [Fact]
        public async Task GetAllBooksSearchIsInOneOtherParamsDefault_ReturnsOne()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello2", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(search: "2");
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 1);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Value, 2);
        }

        [Fact]
        public async Task GetAllBooksFilterByAuthorOtherParamsDefault_ReturnsOne()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello2", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(authorToFilter: "1");
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 1);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Value, 2);
        }

        [Fact]
        public async Task GetAllBooksFilterByGenreOtherParamsDefault_ReturnsBoth()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "2", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now - new TimeSpan(72, 12, 30), LastToReturn = DateTime.Now - new TimeSpan(72, 12, 10) }, new DatabaseBook { Id = "2", Title = "hello2", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var unitOfWork = new UnitOfWork<Book>(_dbContext);
            var service = new LibraryService<Book>(unitOfWork, _mapper, null) as IServiceAsync<Book>;
            var timenow = DateTime.Now;

            var res = await service.GetAllAsync(genreToFilter: "dhs");
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Key.Count(), 2);
            Assert.Equal((res as KeyValuePair<List<Book>, int>?)?.Value, 2);
        }
    }

}