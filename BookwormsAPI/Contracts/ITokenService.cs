using System.Threading.Tasks;
using BookwormsAPI.Entities.Identity;

namespace BookwormsAPI.Contracts
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}