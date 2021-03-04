using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Specifications
{
    public class RequestsForUserWithBookDetailsSpecification : BaseSpecification<Request>
    {
        public RequestsForUserWithBookDetailsSpecification(string borrowerEmail) 
            : base(r => r.BorrowerEmail == borrowerEmail)
        {
            AddInclude(r => r.Book);
            AddInclude(r => r.Book.Author);
            ApplyOrderByDescending(r => r.DateRequested);
        }

    }
}