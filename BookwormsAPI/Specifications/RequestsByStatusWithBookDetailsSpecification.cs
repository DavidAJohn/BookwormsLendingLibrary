using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestsByStatusWithBookDetailsSpecification : BaseSpecification<Request>
    {
        public RequestsByStatusWithBookDetailsSpecification(RequestStatus requestStatus) 
            : base(r => 
                r.Status == requestStatus
                && r.DateReturned == null
                && (DateTime.Now < r.DateDue || r.DateDue == null)
            )
        {
            AddInclude(r => r.Book);
            AddInclude(r => r.Book.Author);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}