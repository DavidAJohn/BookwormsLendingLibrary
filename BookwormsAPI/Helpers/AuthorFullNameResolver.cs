using AutoMapper;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Helpers
{
    public class AuthorFullNameResolver : IValueResolver<Book, BookDTO, string>
    {
        public AuthorFullNameResolver()
        {
        }

        public string Resolve(Book source, BookDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Author.FirstName) && !string.IsNullOrEmpty(source.Author.LastName)) 
            {
                return source.Author.FirstName + ' ' + source.Author.LastName;
            }

            return null;
        }
    }
}