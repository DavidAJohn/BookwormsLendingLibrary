using BookwormsAPI.Entities;

namespace BookwormsAPI.Specifications
{
    public class AuthorsWithFiltersForCountSpecification : BaseSpecification<Author>
    {
        public AuthorsWithFiltersForCountSpecification(AuthorSpecificationParams authorParams)
            : base(a =>
                (string.IsNullOrEmpty(authorParams.Search) || a.LastName.ToLower().Contains(authorParams.Search))
            )
        {
            AddInclude(a => a.Books);
        }
    }
}