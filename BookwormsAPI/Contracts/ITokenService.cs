using BookwormsAPI.Entities.Identity;

namespace BookwormsAPI.Contracts
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}