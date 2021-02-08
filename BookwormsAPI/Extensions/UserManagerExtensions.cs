using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipalWithAddressAsync(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return await input.Users
                .Include(x => x.Address)
                .SingleOrDefaultAsync(x => x.Email == email);
        }

        public static async Task<AppUser> FindUserByEmailFromClaimsPrincipal(this UserManager<AppUser> input, ClaimsPrincipal user)
        {
            var email = user?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return await input.Users
                .SingleOrDefaultAsync(x => x.Email == email);
        }
    }
}
