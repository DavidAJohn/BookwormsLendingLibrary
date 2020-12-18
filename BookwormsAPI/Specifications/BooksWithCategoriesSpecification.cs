using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksWithCategoriesSpecification : BaseSpecification<Book>
    {
        public BooksWithCategoriesSpecification()
        {
            AddInclude(b => b.Category);
        }

        public BooksWithCategoriesSpecification(int id) 
            : base(b => b.Id == id)
        {
            AddInclude(b => b.Category);
        }
    }
}