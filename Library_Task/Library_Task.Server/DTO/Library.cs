using Microsoft.AspNetCore.Identity;

namespace Library_Task.Server.DTO
{
    public interface Entity
    {
        public string Id { get; set; }
    }

    public interface BusinessEntity : Entity, ICloneable;

    public class Book : BusinessEntity
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AuthorId { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? LastTaken { get; set; }
        public DateTime? LastToReturn { get; set; }
        public string? BookImage { get; set; }

        public Book(string id, string iSBN, string genre, string title, string description, DateTime? lastTaken, DateTime? lastToReturn, string? bookImage)
        {
            Id = id;
            ISBN = iSBN;
            Genre = genre;
            Title = title;
            Description = description;
            LastTaken = lastTaken;
            LastToReturn = lastToReturn;
            BookImage = bookImage;
        }

        public object Clone()
        {
            return new Book(Id, ISBN, Genre, Title, Description, LastTaken, LastToReturn, BookImage);
        }
    }

    public class Author : BusinessEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfOrigin { get; set; }

        public Author(string id, string name, string surname, DateTime dateOfBirth, string countryOfOrigin)
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
            CountryOfOrigin = countryOfOrigin;
        }

        public Author() { }

        public object Clone()
        {
            return new Author(Id, Name, Surname, DateOfBirth, CountryOfOrigin);
        }
    }

    public class Reader : IdentityUser, BusinessEntity
    {
        public string Id { get; set; }
        public bool IsAdmin { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Reader() { }

        public Reader(string id, bool isadmin, string email, string password)
        {
            Id = id;
            IsAdmin = isadmin;
            Email = email;
            Password = password;
        }

        public object Clone()
        {
            return new Reader(Id, IsAdmin, Email, Password);
        }
    }
}

