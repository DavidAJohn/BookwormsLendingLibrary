using AutoMapper;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Entities.Borrowing;
using BookwormsAPI.Entities.Identity;

namespace BookwormsAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom<AuthorFullNameResolver>());

            CreateMap<Book, BookForAuthorDTO>();
            CreateMap<BookCreateDTO, Book>();
            CreateMap<BookUpdateDTO, Book>().ReverseMap();

            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorCreateDTO, Author>();
            CreateMap<AuthorUpdateDTO, Author>().ReverseMap();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryCreateDTO, Category>();

            CreateMap<BookwormsAPI.Entities.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, BookwormsAPI.Entities.Borrowing.Address>();

            CreateMap<Request, RequestToReturnDTO>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.BookAuthor, opt => opt.MapFrom(src => src.Book.Author.FirstName + ' ' + src.Book.Author.LastName));
        }
    }
}