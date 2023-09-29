using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class BooksControllerTests : TestBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DefaultHttpContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;
        private BooksController _sut;

        public BooksControllerTests()
        {
            _bookRepository = Substitute.For<IBookRepository>();
            _context = new DefaultHttpContext();
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext = _context;
            _mapper = BuildMap();
            _logger = Substitute.For<ILogger<BooksController>>();

            _sut = new BooksController(_bookRepository, _mapper, _httpContextAccessor, _logger);
        }

        [Fact]
        public async Task GetBooks_ReturnsListOfBooks_WhenCalled()
        {
            // Arrange
            var bookParams = new BookSpecificationParams()
            {
                PageIndex = 1,
                PageSize = 5
            };

            var spec = new BooksWithCategoriesAndAuthorsSpecification(bookParams);
            _bookRepository
                .ListAsync(spec)
                .Returns(new List<Book>());

            // Act
            var result = (OkObjectResult)await _sut.GetBooks(bookParams);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<List<BookDTO>>();
        }

        [Fact]
        public async Task GetBookById_ReturnsBook_WhenBookExists()
        {
            // Arrange
            var book = CreateFakeBook("Book 1");

            _bookRepository
                .GetEntityWithSpec(Arg.Any<BooksWithCategoriesAndAuthorsSpecification>())
                .Returns(book);

            // Act
            var result = (OkObjectResult)await _sut.GetBookById(book.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<BookDTO>();

            var returnedBook = (BookDTO)result.Value;
            returnedBook.Id.Should().Be(book.Id);
        }

        [Fact]
        public async Task GetBookById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            // Arrange
            _bookRepository
                .GetEntityWithSpec(Arg.Any<BooksWithCategoriesAndAuthorsSpecification>())
                .ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.GetBookById(int.MaxValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateBook_ReturnsCreatedResponse_WhenInputIsValid()
        {
            // Arrange
            var bookDTO = new BookCreateDTO
            {
                Title = "Book 1"
            };

            _bookRepository.Create(Arg.Any<Book>()).Returns(new Book());

            // Act
            var result = (CreatedAtRouteResult)await _sut.CreateBook(bookDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.RouteName.Should().Be("GetBookById");
            result.Value.Should().BeOfType<BookForAuthorDTO>();
        }

        [Fact]
        public async Task CreateBook_ReturnsBadRequest_WhenInputIsNull()
        {
            // Act
            var result = (BadRequestObjectResult)await _sut.CreateBook(null);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNoContent_WhenBookIsDeleted()
        {
            // Arrange
            var book = CreateFakeBook("Book 1");
            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            _bookRepository.Delete(book).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.DeleteBook(book.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteBook_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            _bookRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.DeleteBook(int.MaxValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteBook_ReturnsBadRequest_WhenBookIsNotDeleted()
        {
            // Arrange
            var book = CreateFakeBook("Book 1");
            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            _bookRepository.Delete(Arg.Any<Book>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.DeleteBook(book.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNoContent_WhenBookIsUpdated()
        {
            // Arrange
            var book = CreateFakeBook("Book 1");
            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            var bookDTO = new BookUpdateDTO
            {
                Title = "Updated 1"
            };

            _bookRepository.Update(Arg.Any<Book>()).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.UpdateBook(book.Id, bookDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task UpdateBook_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var bookDTO = new BookUpdateDTO
            {
                Title = "Book 1"
            };

            _bookRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.UpdateBook(int.MaxValue, bookDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateBook_ReturnsBadRequest_WhenBookIsNotUpdated()
        {
            // Arrange
            var book = CreateFakeBook("Book 1");
            _bookRepository.GetByIdAsync(book.Id).Returns(book);

            var bookDTO = new BookUpdateDTO
            {
                Title = "Updated 1"
            };

            _bookRepository.Update(Arg.Any<Book>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.UpdateBook(book.Id, bookDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
