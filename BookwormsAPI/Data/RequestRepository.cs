using BookwormsAPI.Contracts;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Data
{
    public class RequestRepository : RepositoryBase<Request>, IRequestRepository
    {
        public RequestRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}