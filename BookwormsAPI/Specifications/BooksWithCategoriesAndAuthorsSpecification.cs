using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksWithCategoriesAndAuthorsSpecification : BaseSpecification<Book>
    {
        public BooksWithCategoriesAndAuthorsSpecification()
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);
        }

        public BooksWithCategoriesAndAuthorsSpecification(int id) 
            : base(b => b.Id == id)
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);
        }
    }
}