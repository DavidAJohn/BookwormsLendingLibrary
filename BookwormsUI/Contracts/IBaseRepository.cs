using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookwormsUI.Contracts
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(string url, int id);
        Task<IList<T>> Get(string url);
        Task<bool> Create(string url, T obj);
        Task<bool> Update(string url, T obj);
        Task<bool> Delete(string url, int id);
    }
}