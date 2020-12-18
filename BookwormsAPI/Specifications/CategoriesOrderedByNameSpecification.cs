using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class CategoriesOrderedByNameSpecification : BaseSpecification<Category>
    {
        public CategoriesOrderedByNameSpecification()
        {
            ApplyOrderBy(c => c.Name);
        }

        public CategoriesOrderedByNameSpecification(int id) 
            : base(c => c.Id == id)
        {
            ApplyOrderBy(c => c.Name);
        }
    }
}