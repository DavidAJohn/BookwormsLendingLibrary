using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class AuthorsWithBooksSpecification : BaseSpecification<Author>
    {
        public AuthorsWithBooksSpecification(AuthorSpecificationParams authorParams)
            : base(a =>
                (string.IsNullOrEmpty(authorParams.Search) || a.LastName.ToLower().Contains(authorParams.Search))
            )
        {
            AddInclude(a => a.Books);
            ApplyPaging(authorParams.PageSize * (authorParams.PageIndex - 1), authorParams.PageSize);

            if (!string.IsNullOrEmpty(authorParams.Sort))
            {
                switch (authorParams.Sort)
                {
                    case "nameAsc":
                        ApplyOrderBy(a => a.LastName);
                        break;
                    case "nameDesc":
                        ApplyOrderByDescending(a => a.LastName);
                        break;
                    default:
                        ApplyOrderBy(a => a.LastName);
                        break;
                }
            }
            else {
                ApplyOrderBy(a => a.LastName);
            }
        }

        public AuthorsWithBooksSpecification(int id) 
            : base(a => a.Id == id)
        {
            AddInclude(a => a.Books);
        }
    }
}