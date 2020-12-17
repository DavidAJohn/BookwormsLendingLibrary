using BookwormsAPI.Contracts;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Data
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}