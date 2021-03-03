using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestsByStatusWithBookDetailsSpecification : BaseSpecification<Request>
    {
        public RequestsByStatusWithBookDetailsSpecification(RequestStatus requestStatus) 
            : base(r => r.Status == requestStatus)
        {
            AddInclude(r => r.Book);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}