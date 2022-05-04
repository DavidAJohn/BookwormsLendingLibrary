using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Data
{
    public class AuthorRepositoryTests : TestBase
    {
        [Fact]
        public async Task GetAuthors_ReturnsListOfAuthors()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.Authors.Add(CreateFakeAuthor("John", "Testman2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new AuthorRepository(context2);
            var authors = await repo.ListAllAsync();

            // Assert
            Assert.NotNull(authors);
            Assert.IsAssignableFrom<List<Author>>(authors);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAnAuthor()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.Authors.Add(CreateFakeAuthor("John", "Testman2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new AuthorRepository(context2);
            var author = await repo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(author);
            Assert.IsType<Author>(author);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsExpectedAuthor()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.Authors.Add(CreateFakeAuthor("John", "Testman2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new AuthorRepository(context2);
            var author = await repo.GetByIdAsync(2);

            // Assert
            Assert.Equal("Testman2", author.LastName);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAuthor()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var newAuthor = CreateFakeAuthor("John", "Testman1");

            // Act
            var repo = new AuthorRepository(context);
            var author = await repo.Create(newAuthor);

            // Assert
            Assert.NotNull(author);
            Assert.IsType<Author>(author);
            Assert.Equal("Testman1", author.LastName);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updAuthor = new Author() { Id = 1, LastName = "Testman-Updated" };

            // Act
            var repo = new AuthorRepository(context2);
            var wasUpdated = await repo.Update(updAuthor);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsExpectedLastName()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updAuthor = new Author() { Id = 1, LastName = "Testman-Updated" };

            // Act
            var repo = new AuthorRepository(context2);
            var wasUpdated = await repo.Update(updAuthor);

            // Assert
            Assert.True(wasUpdated);

            var author = context2.Authors.First();
            Assert.Equal("Testman-Updated", author.LastName);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delAuthor = new Author() { Id = 1, LastName = "Testman1" };

            // Act
            var repo = new AuthorRepository(context2);
            var wasUpdated = await repo.Delete(delAuthor);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task DeleteAuthor_DeletesOneAuthor()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.Authors.Add(CreateFakeAuthor("John", "Testman2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delAuthor = new Author() { Id = 1, LastName = "Testman1" };

            // Act
            var repo = new AuthorRepository(context2);
            var wasUpdated = await repo.Delete(delAuthor);

            // Assert
            Assert.True(wasUpdated);

            var authorCount = context2.Authors.Count();
            Assert.Equal(1, authorCount);
        }

        [Fact]
        public async Task DeleteAuthor_DeletesCorrectAuthor()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Authors.Add(CreateFakeAuthor("John", "Testman1"));
            context.Authors.Add(CreateFakeAuthor("John", "Testman2"));
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delAuthor = new Author() { Id = 1, LastName = "Testman1" };

            // Act
            var repo = new AuthorRepository(context2);
            var wasUpdated = await repo.Delete(delAuthor);

            // Assert
            Assert.True(wasUpdated);

            var author = context2.Authors.First();
            Assert.Equal("Testman2", author.LastName);
        }
    }
}
