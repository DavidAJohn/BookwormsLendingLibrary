using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using BookwormsAPI.Data;
using BookwormsAPI.Data.Identity;
using BookwormsAPI.Entities;
using BookwormsAPI.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Tests
{
    public class TestBase
    {
        protected static ApplicationDbContext BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected static AppIdentityDbContext BuildIdentityContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<AppIdentityDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            var dbContext = new AppIdentityDbContext(options);

            return dbContext;
        }

        protected static IMapper BuildMap()
        {
            var config = new MapperConfiguration(options =>
            {
                options.AddProfile(new MappingProfiles());
            });

            return config.CreateMapper();
        }

        protected static ControllerContext BuildControllerContextWithDefaultUser()
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

        protected static ControllerContext BuildControllerContextWithAdminUser()
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

        protected static Book CreateFakeBook(string title)
        {
            var testAuthor = new Author() { FirstName = "John", LastName = "Testman" };
            var testCategory = new Category() { Name = "Category 1" };

            // Act
            var testBook = new Book()
            {
                Title = title,
                YearPublished = 2022,
                ISBN = "ISBN9999",
                Summary = "Summary",
                CoverImageUrl = "https://image.url",
                AuthorId = 1,
                Author = testAuthor,
                CategoryId = 1,
                Category = testCategory
            };

            return testBook;
        }

        protected static Author CreateFakeAuthor(string firstName, string lastName)
        {
            var books = new List<Book>();

            var book1 = new Book()
            {
                Title = "Title 1",
                Summary = "Summary"
            };

            var book2 = new Book()
            {
                Title = "Title 2",
                Summary = "Summary"
            };

            books.Add(book1);
            books.Add(book2);

            var testAuthor = new Author()
            {
                FirstName = firstName,
                LastName = lastName,
                Biography = "John's Biography",
                AuthorImageUrl = "https://image.url",
                Books = books
            };

            return testAuthor;
        }
    }
}