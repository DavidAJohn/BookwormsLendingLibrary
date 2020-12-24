using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksOrderedByTitleOrAuthorSpecification : BaseSpecification<Book>
    {
        public BooksOrderedByTitleOrAuthorSpecification(string sort)
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "titleAsc":
                        ApplyOrderBy(b => b.Title);
                        break;
                    case "titleDesc":
                        ApplyOrderByDescending(b => b.Title);
                        break;
                    case "authorAsc":
                        ApplyOrderBy(b => b.Author.LastName);
                        break;
                    case "authorDesc":
                        ApplyOrderByDescending(b => b.Author.LastName);
                        break;
                    case "yearPublishedAsc":
                        ApplyOrderBy(b => b.YearPublished);
                        break;
                    case "yearPublishedDesc":
                        ApplyOrderByDescending(b => b.YearPublished);
                        break;
                    default:
                        ApplyOrderBy(b => b.Title);
                        break;
                }
            }
            else {
                ApplyOrderBy(b => b.Title);
            }
        }

    }
}