using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksWithFiltersForCountSpecification : BaseSpecification<Book>
    {
        public BooksWithFiltersForCountSpecification(BookSpecificationParams bookParams)
            : base(b =>
                (string.IsNullOrEmpty(bookParams.Search) || b.Title.ToLower().Contains(bookParams.Search)) &&
                (!bookParams.AuthorId.HasValue || b.AuthorId == bookParams.AuthorId) &&
                (!bookParams.CategoryId.HasValue || b.CategoryId == bookParams.CategoryId)
            )
        {
        }
    }
}