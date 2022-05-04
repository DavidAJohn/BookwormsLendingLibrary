using BookwormsAPI.Entities;
using System;
using System.Linq;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Entities
{
    public class BookTests : TestBase
    {
        [Fact]
        public void CreateBook_ReturnsBookWithExpectedTitle()
        {
            // Arrange
            var testAuthor = new Author() { Id = 1, FirstName = "John", LastName = "Testman" };
            var testCategory = new Category() { Id = 1, Name = "Category 1" };

            // Act
            var testBook = new Book()
            {
                Title = "Title 1",
                YearPublished = 2022,
                ISBN = "ISBN9999",
                Summary = "Summary",
                CoverImageUrl = "https://image.url",
                AuthorId = 1,
                Author = testAuthor,
                CategoryId = 1,
                Category = testCategory
            };

            // Assert
            Assert.NotNull(testBook);
            Assert.Equal("Title 1", testBook.Title);
        }

        [Fact]
        public void CreateBook_ReturnsBookWithIsActiveTrue()
        {
            // Arrange

            // Act
            var testBook = new Book() { Title = "Title 1" };

            // Assert
            Assert.NotNull(testBook);
            Assert.True(testBook.IsActive);
        }
    }
}
