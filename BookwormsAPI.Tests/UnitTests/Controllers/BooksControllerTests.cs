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
    public class BooksControllerTests : TestBase
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public BooksControllerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        }

        [Fact]
        public async Task GetBooks_ReturnsOkResult()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(context);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);
            var bookParams = new BookSpecificationParams()
            {
                PageIndex = 1,
                PageSize = 5
            };

            // Act
            var response = await controller.GetBooks(bookParams);

            // Assert
            Assert.IsType<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task GetBookById_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.GetBookById(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response.Result);
        }

        [Fact]
        public async Task CreateBook_WhenInputIsNull_ReturnsBadRequest()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            BookCreateDTO bookDTO = null;

            // Act
            var response = await controller.CreateBook(bookDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(response.Result);
        }

        [Fact]
        public async Task DeleteBook_ReturnsActionResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.DeleteBook(1);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task DeleteBook_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            // Act
            var response = await controller.DeleteBook(9999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact]
        public async Task UpdateBook_ReturnsActionResult()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            var bookDTO = new BookUpdateDTO
            {
                Title = "Book 1"
            };

            // Act
            var response = await controller.UpdateBook(1, bookDTO);

            // Assert
            Assert.IsAssignableFrom<ActionResult>(response);
        }

        [Fact]
        public async Task UpdateBook_Id_NonExistent_ReturnsNotFound()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            var controller = new BooksController(_bookRepositoryMock.Object, _mapperMock.Object, _httpContextAccessorMock.Object);

            var bookDTO = new BookUpdateDTO
            {
                Title = "Book 1"
            };

            // Act
            var response = await controller.UpdateBook(9999, bookDTO);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
        }
    }
}
