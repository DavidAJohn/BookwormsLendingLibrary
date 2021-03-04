using System.Security.Claims;

namespace BookwormsAPI.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmailFromClaimsPrincipal(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email);
        }
    }
}