using BookwormsAPI.Entities;
using System.Collections.Generic;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Entities
{
    public class AuthorTests : TestBase
    {
        [Fact]
        public void CreateAuthor_AuthorCreatedWithTwoBooks()
        {
            // Arrange
            var books = new List<Book>();

            var book1 = new Book()
            {
                Id = 1,
                Title = "Title 1"
            };

            var book2 = new Book()
            {
                Id = 2,
                Title = "Title 2"
            };

            books.Add(book1);
            books.Add(book2);

            // Act
            var testAuthor = new Author()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Testman",
                Biography = "John's Biography",
                AuthorImageUrl = "https://image.url",
                Books = books
            };

            // Assert
            Assert.NotNull(testAuthor.Books);
            Assert.Equal(2, testAuthor.Books.Count);
        }

        [Fact]
        public void CreateAuthor_AuthorCreatedWithExpectedFirstName()
        {
            // Arrange

            // Act
            var testAuthor = new Author() { Id = 1, FirstName = "John", LastName = "Testman" };

            // Assert
            Assert.NotNull(testAuthor);
            Assert.Equal("John", testAuthor.FirstName);
        }

        [Fact]
        public void CreateAuthor_AuthorCreatedWithIsActiveTrue()
        {
            // Arrange

            // Act
            var testAuthor = new Author() { Id = 1, FirstName = "John", LastName = "Testman" };

            // Assert
            Assert.NotNull(testAuthor);
            Assert.True(testAuthor.isActive);
        }
    }
}
