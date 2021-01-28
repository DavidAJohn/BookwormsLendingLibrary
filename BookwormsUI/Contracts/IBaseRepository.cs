using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsUI.Models;

namespace BookwormsUI.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string url, int id);
        Task<PagedList<T>> GetAsync(string url, ItemParameters itemParams);
        Task<bool> CreateAsync(string url, T obj);
        Task<bool> UpdateAsync(string url, T obj);
        Task<bool> DeleteAsync(string url, int id);
    }
}