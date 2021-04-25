using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsUI.Models;

namespace BookwormsUI.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<List<Category>> GetListAsync(string url);
    }
}