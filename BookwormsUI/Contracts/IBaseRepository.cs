using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookwormsUI.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string url, int id);
        Task<IList<T>> GetAsync(string url);
        Task<bool> CreateAsync(string url, T obj);
        Task<bool> UpdateAsync(string url, T obj);
        Task<bool> DeleteAsync(string url, int id);
    }
}