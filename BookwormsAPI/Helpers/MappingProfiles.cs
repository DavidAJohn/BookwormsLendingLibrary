using AutoMapper;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Book, BookToReturnDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author.FirstName + ' ' + src.Author.LastName));
        }
    }
}