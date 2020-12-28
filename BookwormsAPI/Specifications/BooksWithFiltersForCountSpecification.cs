using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class BooksWithFiltersForCountSpecification : BaseSpecification<Book>
    {
        public BooksWithFiltersForCountSpecification(BookSpecificationParams bookParams)
            : base(b =>
                (!bookParams.AuthorId.HasValue || b.AuthorId == bookParams.AuthorId) &&
                (!bookParams.CategoryId.HasValue || b.CategoryId == bookParams.CategoryId)
            )
        {
        }
    }
}