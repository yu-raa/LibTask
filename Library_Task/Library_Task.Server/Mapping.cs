using AutoMapper;
using Library_Task.Server.DTO;
using Modsen_Library_Test_Task.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reader, DatabaseUser>();
        CreateMap<DatabaseUser, Reader>();
        CreateMap<Book, DatabaseBook>();
        CreateMap<Author, DatabaseAuthor>();
        CreateMap<DatabaseBook, Book>();
        CreateMap<DatabaseAuthor, Author>();
    }
}