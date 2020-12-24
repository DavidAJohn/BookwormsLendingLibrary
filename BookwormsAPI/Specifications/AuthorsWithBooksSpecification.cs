using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class AuthorsWithBooksSpecification : BaseSpecification<Author>
    {
        public AuthorsWithBooksSpecification()
        {
            AddInclude(a => a.Books);
            ApplyOrderBy(a => a.LastName);
        }

        public AuthorsWithBooksSpecification(int id) 
            : base(a => a.Id == id)
        {
            AddInclude(a => a.Books);
            ApplyOrderBy(a => a.LastName);
        }
    }
}