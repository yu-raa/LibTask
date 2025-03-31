using AutoMapper;
using Library_Task.Server.DTO;
using Library_Task.Server.Frameworks;
using Library_Task.Server.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Modsen_Library_Test_Task.Entities;
using Moq;

namespace ServerTests
{
    public class UnitTestRepo
    {
        private readonly LibraryContext _dbContext;

        public UnitTestRepo()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>().UseInMemoryDatabase("TestDatabase2").ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
            _dbContext = new LibraryContext(options);
        }

        [Fact]
        public async Task GetBookByWrongId_ReturnsNull()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var repo = new Repository<DatabaseBook>(_dbContext);

            var res = await repo.GetById("");
            Assert.Null(res);
        }

        [Fact]
        public async Task GetBookByRightId_ReturnsSomething()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });
            _dbContext.AddRange(new DatabaseBook { Id = "1", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "1234567890123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) }, new DatabaseBook { Id = "2", Title = "hello", AuthorId = "2", UserId = "1", Description = "cdhch", Genre = "dhs", ISBN = "1234567200123", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) });

            _dbContext.SaveChanges();

            var repo = new Repository<DatabaseBook>(_dbContext);

            var res = await repo.GetById("1");

            Assert.NotNull(res);
        }

        [Fact]
        public async Task UpdateBookPassNull_DoesNotUpdate()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });

            _dbContext.SaveChanges();

            var repo = new Repository<DatabaseBook>(_dbContext);

            var newBook = new DatabaseBook { Id = "5", Title = "hello", AuthorId = "1", UserId = "1", Description = "cdhjdch", Genre = "dhs", ISBN = "123456789012345", LastTaken = DateTime.Now, LastToReturn = DateTime.Now + new TimeSpan(72, 12, 30) };

            await Assert.ThrowsAsync<ArgumentNullException>(() => repo.Alter(default(DatabaseBook)));
        }

        [Fact]
        public async Task UpdateBookPassNotNull_DoesUpdate()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();

            _dbContext.AddRange(new DatabaseUser { Id = "1", Email = "qwer@yandex.by", IsAdmin = false, Password = "hjwwdhdhjwdhgxQ1542_", LockoutEnd = DateTime.MaxValue }, new DatabaseUser { Id = "2", Email = "qwerty@yandex.by", IsAdmin = true, Password = "hjwwdhdhjwdhg3Q1542_" }, new DatabaseUser { Id = "3", Email = "qwe@yandex.by", IsAdmin = false, Password = "hjwwdhdhjg3Q1542_" });
            _dbContext.AddRange(new DatabaseAuthor { Id = "1", CountryOfOrigin = "hdvhj", DateOfBirth = new DateTime(2000, 10, 9), Name = "ergh", Surname = "dhjdchkjd" }, new DatabaseAuthor { Id = "2", Surname = "eghwjehj", Name = "etyw", DateOfBirth = new DateTime(1929, 2, 12), CountryOfOrigin = "whedhg" });

            _dbContext.SaveChanges();

            var repo = new Repository<DatabaseBook>(_dbContext);

            var oldBook = repo.GetById("1");

            Assert.NotNull(repo.Alter(oldBook));
        }
    }

}