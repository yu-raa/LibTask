using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modsen_Library_Test_Task.Entities;

namespace Library_Task.Server.Frameworks
{
    public class LibraryContext : IdentityDbContext
    {

        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DatabaseUser>().ToTable("AspNetUsers");
            modelBuilder.Entity<DatabaseUser>()
                .Property(user => user.Email).IsRequired();
            modelBuilder.Entity<DatabaseUser>()
                .Property(user => user.Password).IsRequired();
            modelBuilder.Entity<DatabaseBook>()
                .Property(book => book.LastTaken)
                .HasColumnType("datetime");
            modelBuilder.Entity<DatabaseBook>()
                .Property(book => book.LastToReturn)
                .HasColumnType("datetime");
            modelBuilder.Entity<DatabaseAuthor>()
                .Property(auth => auth.DateOfBirth)
                .HasColumnType("datetime");
            modelBuilder.Entity<DatabaseUser>().Property(user => user.Id).ValueGeneratedNever();
            modelBuilder.Entity<DatabaseUser>().HasIndex(user => user.Email).IsUnique();
        }
    }
}