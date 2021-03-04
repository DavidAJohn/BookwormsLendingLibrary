using System;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestOutstandingSpecification : BaseSpecification<Request>
    {
        public RequestOutstandingSpecification(int bookID, string borrowerEmail) 
            : base(
                r => r.BookId == bookID && 
                r.BorrowerEmail == borrowerEmail &&
                (r.Status == RequestStatus.Pending ||
                    (r.DateDue != null && DateTime.Now > r.DateDue)
                )
            )
        {
            AddInclude(r => r.Book);
            AddInclude(r => r.Book.Author);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}