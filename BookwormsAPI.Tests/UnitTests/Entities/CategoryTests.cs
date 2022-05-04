using BookwormsAPI.Entities;
using System;
using System.Linq;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Entities
{
    public class CategoryTests : TestBase
    {
        [Fact]
        public void CreateCategory_ReturnsCategoryWithExpectedName()
        {
            // Arrange

            // Act
            var testCategory = new Category() { Name = "Category 1" };

            // Assert
            Assert.NotNull(testCategory);
            Assert.Equal("Category 1", testCategory.Name);
        }

        [Fact]
        public void CreateCategory_ReturnsCategoryWithIsActiveTrue()
        {
            // Arrange

            // Act
            var testCategory = new Category() { Name = "Category 1" };

            // Assert
            Assert.NotNull(testCategory);
            Assert.True(testCategory.isActive);
        }
    }
}
