using System;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestsOverdueSpecification : BaseSpecification<Request>
    {
        public RequestsOverdueSpecification() 
            : base(r => 
                r.DateDue != null 
                && DateTime.Now > r.DateDue
                && r.DateReturned == null
            )
        {
            AddInclude(r => r.Book);
            AddInclude(r => r.Book.Author);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}
