using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;

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

            CreateMap<Author, AuthorDTO>();

            CreateMap<Category, CategoryDTO>();
        }
    }
}