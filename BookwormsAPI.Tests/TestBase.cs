using System.Security.Claims;
using AutoMapper;
using BookwormsAPI.Data;
using BookwormsAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Tests
{
    public class TestBase
    {
        protected ApplicationDbContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected IMapper BuildMap()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new MappingProfiles());
            });

            return config.CreateMapper();
        }

        protected ControllerContext BuildControllerContextWithDefaultUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                new Claim(ClaimTypes.Email, "david@test.com"),
                new Claim(ClaimTypes.Role, "Borrower"),
               }, "test"));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        protected ControllerContext BuildControllerContextWithAdminUser()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                new Claim(ClaimTypes.Email, "admin@test.com"),
                new Claim(ClaimTypes.Role, "Administrator"),
               }, "test"));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}