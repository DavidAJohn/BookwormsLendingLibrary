using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Data
{
    public class CategoryRepositoryTests : TestBase
    {
        [Fact]
        public async Task GetCategories_ReturnsListOfCategories()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
            context.Categories.Add(new Category() { Id = 2, Name = "Category 2" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new CategoryRepository(context2);
            var categories = await repo.ListAllAsync();

            // Assert
            Assert.NotNull(categories);
            Assert.IsAssignableFrom<List<Category>>(categories);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsACategory()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
            context.Categories.Add(new Category() { Id = 2, Name = "Category 2" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new CategoryRepository(context2);
            var category = await repo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(category);
            Assert.IsType<Category>(category);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsExpectedCategory()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
            context.Categories.Add(new Category() { Id = 2, Name = "Category 2" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);

            // Act
            var repo = new CategoryRepository(context2);
            var category = await repo.GetByIdAsync(2);

            // Assert
            Assert.Equal("Category 2", category.Name);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCreatedCategory()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            var newCategory = new Category() { Id = 1, Name = "Created Category" };

            // Act
            var repo = new CategoryRepository(context);
            var category = await repo.Create(newCategory);

            // Assert
            Assert.NotNull(category);
            Assert.IsType<Category>(category);
            Assert.Equal("Created Category", category.Name);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updCategory = new Category() { Id = 1, Name = "Updated Category" };

            // Act
            var repo = new CategoryRepository(context2);
            var wasUpdated = await repo.Update(updCategory);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsExpectedName()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category 1" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var updCategory = new Category() { Id = 1, Name = "Updated Category" };

            // Act
            var repo = new CategoryRepository(context2);
            var wasUpdated = await repo.Update(updCategory);

            // Assert
            Assert.True(wasUpdated);

            var category = context2.Categories.First();
            Assert.Equal("Updated Category", category.Name);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsTrue()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category To Delete" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delCategory = new Category() { Id = 1, Name = "Category To Delete" };

            // Act
            var repo = new CategoryRepository(context2);
            var wasUpdated = await repo.Delete(delCategory);

            // Assert
            Assert.True(wasUpdated);
        }

        [Fact]
        public async Task DeleteCategory_DeletesOneCategory()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category To Leave" });
            context.Categories.Add(new Category() { Id = 2, Name = "Category To Delete" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delCategory = new Category() { Id = 2, Name = "Category To Delete" };

            // Act
            var repo = new CategoryRepository(context2);
            var wasUpdated = await repo.Delete(delCategory);

            // Assert
            Assert.True(wasUpdated);

            var categoryCount = context2.Categories.Count();
            Assert.Equal(1, categoryCount);
        }

        [Fact]
        public async Task DeleteCategory_DeletesCorrectCategory()
        {
            // Arrange
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);

            context.Categories.Add(new Category() { Id = 1, Name = "Category To Leave" });
            context.Categories.Add(new Category() { Id = 2, Name = "Category To Delete" });
            context.SaveChanges();

            var context2 = BuildContext(databaseName);
            var delCategory = new Category() { Id = 2, Name = "Category To Delete" };

            // Act
            var repo = new CategoryRepository(context2);
            var wasUpdated = await repo.Delete(delCategory);

            // Assert
            Assert.True(wasUpdated);

            var category = context2.Categories.First();
            Assert.Equal("Category To Leave", category.Name);
        }
    }
}
