using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestByIdWithBookDetailsSpecification : BaseSpecification<Request>
    {
        public RequestByIdWithBookDetailsSpecification(int id) 
            : base(r => r.Id == id)
        {
            AddInclude(r => r.Book);
            AddInclude(r => r.Book.Author);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}