using Library_Task.Server.DTO;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Modsen_Library_Test_Task.Entities
{
    public interface DatabaseItem : Entity;

    public class DatabaseBook : DatabaseItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? AuthorId { get; set; }
        public string? UserId { get; set; }
        public DateTime LastTaken { get; set; }
        public DateTime LastToReturn { get; set; }
        public string? BookImage { get; set; }
    }

    public class DatabaseAuthor : DatabaseItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfOrigin { get; set; }
    }

    public class DatabaseUser : IdentityUser, DatabaseItem
    {
        public bool IsAdmin { get; set; }
        public string Password { get; set; }
    }
}