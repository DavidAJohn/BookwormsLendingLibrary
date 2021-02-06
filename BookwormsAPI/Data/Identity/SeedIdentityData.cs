using System.Linq;
using System.Threading.Tasks;
using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookwormsAPI.Data.Identity
{
    public class SeedIdentityData
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "David",
                    Email = "david@test.com",
                    UserName = "david@test.com",
                    Address = new Address
                    {
                        FirstName = "David",
                        LastName = "Jones",
                        Street = "40 Stansfield Road",
                        City = "Brixton",
                        County = "London",
                        PostCode = "SW9"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}