using System.Threading.Tasks;
using BookwormsUI.Models;

namespace BookwormsUI.Contracts
{
    public interface IAdminService
    {
        Task<SiteStatusTotals> GetSiteStatusAsync();
    }
}