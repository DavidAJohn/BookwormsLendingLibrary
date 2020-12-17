using BookwormsAPI.Contracts;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Data
{
    public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
    {
        public AuthorRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}