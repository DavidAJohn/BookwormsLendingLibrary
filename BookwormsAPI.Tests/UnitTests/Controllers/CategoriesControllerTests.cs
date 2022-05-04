using BookwormsAPI.Contracts;
using BookwormsAPI.Controllers;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class CategoriesControllerTests : TestBase
    {
        [Fact]
        public async Task GetCategories_ReturnsOkResult()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            // Act
            var response = await controller.GetCategories();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task GetCategoryById_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            // Act
            var response = await controller.GetCategoryById(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task CreateCategory_WhenCalled_ReturnsActionResultOfCategoryDTO()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            // Act
            var response = await controller.CreateCategory(catDTO);

            // Assert
            Assert.IsType<ActionResult<CategoryDTO>>(response);
        }

        [Fact]
        public async Task CreateCategory_WhenInputIsNull_ReturnsBadRequest()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            CategoryCreateDTO catDTO = null;

            // Act
            var response = await controller.CreateCategory(catDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsActionResult()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            // Act
            var response = await controller.DeleteCategory(1);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task DeleteCategory_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            // Act
            var response = await controller.DeleteCategory(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsActionResult()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            // Act
            var response = await controller.UpdateCategory(1, catDTO);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task UpdateCategory_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var mapper = BuildMap();
            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            // Act
            var response = await controller.UpdateCategory(9999, catDTO);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }
    }
}