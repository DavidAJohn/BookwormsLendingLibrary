using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Data
{
    public class BookRepositoryTests : TestBase
    {
        [Fact]
        public async Task GetBooks_ReturnsListOfBooks()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Title 1"));
            context.Books.Add(CreateFakeBook("Title 2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new BookRepository(context2);
            var books = await repo.ListAllAsync();

            // Assert
            Assert.NotNull(books);
            Assert.IsAssignableFrom<List<Book>>(books);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsABook()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Title 1"));
            context.Books.Add(CreateFakeBook("Title 2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new BookRepository(context2);
            var book = await repo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(book);
            Assert.IsType<Book>(book);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsExpectedBook()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Title 1"));
            context.Books.Add(CreateFakeBook("Title 2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new BookRepository(context2);
            var book = await repo.GetByIdAsync(2);

            // Assert
            Assert.Equal("Title 2", book.Title);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedBook()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var newBook = CreateFakeBook("Created Title");

            // Act
            var repo = new BookRepository(context);
            var book = await repo.Create(newBook);

            // Assert
            Assert.NotNull(book);
            Assert.IsType<Book>(book);
            Assert.Equal("Created Title", book.Title);
        }

        [Fact]
        public async Task UpdateBook_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Title 1"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updBook = new Book() { Id = 1, Title = "Updated Title" };

            // Act
            var repo = new BookRepository(context2);
            var wasUpdated = await repo.Update(updBook);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task UpdateBook_ReturnsExpectedTitle()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Title 1"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updBook = new Book() { Id = 1, Title = "Updated Title" };

            // Act
            var repo = new BookRepository(context2);
            var wasUpdated = await repo.Update(updBook);

            // Assert
            Assert.True(wasUpdated);

            var book = context2.Books.First();
            Assert.Equal("Updated Title", book.Title);
        }

        [Fact]
        public async Task DeleteBook_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Book To Delete"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delBook = new Book() { Id = 1, Title = "Book To Delete" };

            // Act
            var repo = new BookRepository(context2);
            var wasUpdated = await repo.Delete(delBook);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task DeleteBook_DeletesOneBook()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Book To Leave"));
            context.Books.Add(CreateFakeBook("Book To Delete"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delBook = new Book() { Id = 2, Title = "Book To Delete" };

            // Act
            var repo = new BookRepository(context2);
            var wasUpdated = await repo.Delete(delBook);

            // Assert
            Assert.True(wasUpdated);

            var bookCount = context2.Books.Count();
            Assert.Equal(1, bookCount);
        }

        [Fact]
        public async Task DeleteBook_DeletesCorrectBook()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Books.Add(CreateFakeBook("Book To Leave"));
            context.Books.Add(CreateFakeBook("Book To Delete"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delBook = new Book() { Id = 2, Title = "Book To Delete" };

            // Act
            var repo = new BookRepository(context2);
            var wasUpdated = await repo.Delete(delBook);

            // Assert
            Assert.True(wasUpdated);

            var book = context2.Books.First();
            Assert.Equal("Book To Leave", book.Title);
        }
    }
}
