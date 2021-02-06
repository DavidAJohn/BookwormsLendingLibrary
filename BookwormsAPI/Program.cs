using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookwormsAPI.Data;
using BookwormsAPI.Data.Identity;
using BookwormsAPI.Entities.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookwormsAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    await context.Database.MigrateAsync();
                    
                    await SeedInitialData.SeedAuthorsDataAsync(context);
                    await SeedInitialData.SeedCategoriesDataAsync(context);
                    await SeedInitialData.SeedBooksDataAsync(context);

                    // Identity
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();

                    await identityContext.Database.MigrateAsync();
                    await SeedIdentityData.SeedUsersAsync(userManager);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during data seeding");
                }
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
