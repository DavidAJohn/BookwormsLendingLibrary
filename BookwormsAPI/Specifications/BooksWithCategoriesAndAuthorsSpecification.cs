using System;
using System.Linq.Expressions;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksWithCategoriesAndAuthorsSpecification : BaseSpecification<Book>
    {
        public BooksWithCategoriesAndAuthorsSpecification(string sort, int? authorId, int? categoryId)
            : base(b =>
                (!authorId.HasValue || b.AuthorId == authorId) &&
                (!categoryId.HasValue || b.CategoryId == categoryId)
            )
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

        public BooksWithCategoriesAndAuthorsSpecification(int id) 
            : base(b => b.Id == id)
        {
            AddInclude(b => b.Category);
            AddInclude(b => b.Author);
        }
    }
}