using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.Controllers;
using BookwormsAPI.DTOs;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class AuthorsControllerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public AuthorsControllerTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetAuthors_ReturnsOkResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);
            var authorParams = new AuthorSpecificationParams()
            {
                PageIndex = 1,
                PageSize = 5
            };

            // Act
            var response = await controller.GetAuthors(authorParams);

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task GetAuthorById_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.GetAuthorById(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task CreateAuthor_WhenInputIsNull_ReturnsBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            AuthorCreateDTO authorDTO = null;

            // Act
            var response = await controller.CreateAuthor(authorDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsActionResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.DeleteAuthor(1);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task DeleteAuthor_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.DeleteAuthor(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsActionResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            var authorDTO = new AuthorUpdateDTO
            {
                FirstName = "John",
                LastName = "White"
            };

            // Act
            var response = await controller.UpdateAuthor(1, authorDTO);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task UpdateAuthor_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new AuthorsController(_authorRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            var authorDTO = new AuthorUpdateDTO
            {
                FirstName = "John",
                LastName = "White"
            };

            // Act
            var response = await controller.UpdateAuthor(1, authorDTO);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }
    }
}
