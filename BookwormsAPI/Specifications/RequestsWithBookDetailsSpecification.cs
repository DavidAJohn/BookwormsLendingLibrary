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
            ApplyOrderByDescending(r => r.DateRequested);
        }

        public RequestsForUserWithBookDetailsSpecification(int id, string borrowerEmail) 
            : base(
                r => r.Id == id && 
                r.BorrowerEmail == borrowerEmail
            )
        {
            AddInclude(r => r.Book);
            ApplyOrderByDescending(r => r.DateRequested);
        }
    }
}