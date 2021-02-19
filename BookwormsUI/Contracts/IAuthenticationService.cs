using System.Threading.Tasks;
using BookwormsUI.Models;

namespace BookwormsUI.Contracts
{
    public interface IAuthenticationService
    {
        Task<LoginResult> Login(LoginModel loginModel);
        Task Logout();
        Task<RegisterResult> Register(RegisterModel registerModel);
    }
}