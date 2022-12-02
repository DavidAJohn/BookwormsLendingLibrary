using BookwormsUI.Models;

namespace BookwormsUI.Contracts;

public interface ICategoryService : IBaseService<Category>
{
    Task<List<Category>> GetListAsync(string url);
    string GetCategoriesApiEndpoint();
}
