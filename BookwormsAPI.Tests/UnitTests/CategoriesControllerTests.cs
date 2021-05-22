using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.Controllers;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests
{
    public class CategoriesControllerTests : TestBase
    {
        private readonly Mock<ICategoryRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoriesController _controller;
        public CategoriesControllerTests()
        {
            _mockRepo = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CategoriesController(_mockRepo.Object, _mockMapper.Object);
        }

        // [Fact]
        // public async Task Create_WhenCalled_CreateCategoryCalledOnce()
        // {
        //     Category cat = null;
        //     _mockRepo.Setup(r => r.Create(It.IsAny<Category>()))
        //         .Callback<Category>(c => cat = c);

        //     var category = new CategoryCreateDTO
        //     {
        //         Name = "Test Category"
        //     };

        //     await _controller.CreateCategory(category); // problem

        //     _mockRepo.Verify(x => x.Create(It.IsAny<Category>()), Times.Once);

        //     Assert.Equal(cat.Name, category.Name);
        // }

        [Fact]
        public async Task CreateCategory_WhenCalled_CreatesCategory()
        {
            Category cat = null;
            _mockRepo.Setup(r => r.Create(cat).Result).Returns(new Category{ Id = 1, Name = "Category 1"});

            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            var response = await _controller.CreateCategory(catDTO);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkResult()
        {
            // Arrange
            // var databaseName = Guid.NewGuid().ToString();
            // var context = BuildContext(databaseName);
            var mapper = BuildMap();

            // context.Categories.Add(new Category() { Name = "Category 1" });
            // context.Categories.Add(new Category() { Name = "Category 2" });
            // context.SaveChanges();

            var mockRepo = new Mock<ICategoryRepository>();
            var controller = new CategoriesController(mockRepo.Object, mapper);

            // Act
            var response = await controller.GetCategories();

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound()
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

        // [Fact]
        // public async Task GetCategories_ReturnsTwoItems()
        // {
        //     // Arrange
        //     var mapper = BuildMap();
        //     var _mockRepo = new Mock<ICategoryRepository>();

        //     Category cat = null;
        //     _mockRepo.Setup(r => r.Create(It.IsAny<Category>()))
        //         .Callback<Category>(c => cat = c);

        //     var category = new CategoryCreateDTO
        //     {
        //         Name = "Test Category"
        //     };

        //     await _controller.CreateCategory(category);
            
        //     // Act
        //     var controller = new CategoriesController(_mockRepo.Object, mapper);
        //     ActionResult<IEnumerable<Category>> categories = await controller.GetCategories();
        //     int categoryCount = categories.Value.ToList().Count;

        //     // Assert
        //     Assert.Equal(2, categoryCount);
        // }
    }
}