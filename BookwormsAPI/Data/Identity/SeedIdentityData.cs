using System.Linq;
using System.Threading.Tasks;
using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BookwormsAPI.Data.Identity
{
    public class SeedIdentityData
    {
        private static IConfiguration _config;

        public static async Task SeedUsersAndRolesAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _config = config;
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    var role = new IdentityRole
                    {
                        Name = "Administrator"
                    };

                    await roleManager.CreateAsync(role);
                }

                if (!await roleManager.RoleExistsAsync("Borrower"))
                {
                    var role = new IdentityRole
                    {
                        Name = "Borrower"
                    };

                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                // create the admin user
                string adminEmail = _config["AdminAuthentication:Email"];
                string adminPassword = _config["AdminAuthentication:Password"];
                
                var adminUser = new AppUser
                {
                    DisplayName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    Address = new Address
                    {
                        FirstName = "Admin",
                        LastName = "User",
                        Street = "1 Library Street",
                        City = "Bromley",
                        County = "London",
                        PostCode = "SW9 1BL"
                    }
                };

                var adminResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (adminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }

                // create a standard user/borrower
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
                        PostCode = "SW9 1DJ"
                    }
                };

                var userResult = await userManager.CreateAsync(user, "Pa$$w0rd");

                if (userResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Borrower");
                }
            }
        }
    }
}