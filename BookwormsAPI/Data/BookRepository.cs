using BookwormsAPI.Contracts;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Data
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}