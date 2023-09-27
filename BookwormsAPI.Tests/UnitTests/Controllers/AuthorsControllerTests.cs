using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class AuthorsControllerTests : TestBase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DefaultHttpContext _context;
        private readonly IMapper _mapper;
        private AuthorsController _sut;

        public AuthorsControllerTests()
        {
            _authorRepository = Substitute.For<IAuthorRepository>();
            _context = new DefaultHttpContext();
            _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            _httpContextAccessor.HttpContext = _context;
            _mapper = BuildMap();

            _sut = new AuthorsController(_authorRepository, _mapper, _httpContextAccessor);
        }

        [Fact]
        public async Task GetAuthors_ReturnsListOfAuthorDTOs_WhenCalled()
        {
            // Arrange
            var authorParams = new AuthorSpecificationParams()
            {
                PageIndex = 1,
                PageSize = 5
            };

            // Act
            var result = (OkObjectResult)await _sut.GetAuthors(authorParams);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsAuthorDTO_WhenIdExists()
        {
            // Arrange
            var author = new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Test"
            };

            _authorRepository
                .GetEntityWithSpec(Arg.Any<AuthorsWithBooksSpecification>())
                .Returns(author);

            // Act
            var result = (OkObjectResult)await _sut.GetAuthorById(1);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetAuthorById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            _authorRepository
                .GetEntityWithSpec(Arg.Any<AuthorsWithBooksSpecification>())
                .ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.GetAuthorById(int.MaxValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsCreatedResponse_WhenInputIsValid()
        {
            // Arrange
            var authorDTO = new AuthorCreateDTO
            {
                FirstName = "John",
                LastName = "Test"
            };

            var author = CreateFakeAuthor("John", "Test");

            _authorRepository
                .Create(Arg.Any<Author>())
                .Returns(author);

            // Act
            var result = (CreatedAtRouteResult)await _sut.CreateAuthor(authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.RouteName.Should().Be("GetAuthorById");
            result.Value.Should().BeOfType<AuthorDTO>();
        }

        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenInputIsInvalid()
        {
            // Arrange
            var authorDTO = new AuthorCreateDTO
            {
                FirstName = "John",
                // LastName is null
            };

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateAuthor(authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenInputIsNull()
        {
            // Arrange
            AuthorCreateDTO authorDTO = null;

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateAuthor(authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenAuthorCouldNotBeCreated()
        {
            // Arrange
            var authorDTO = new AuthorCreateDTO
            {
                FirstName = "John",
                LastName = "Test"
            };

            _authorRepository
                .Create(Arg.Any<Author>())
                .ReturnsNull();

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateAuthor(authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNoContent_WhenAuthorIsDeleted()
        {
            // Arrange
            var author = CreateFakeAuthor("John", "Test");
            _authorRepository.GetByIdAsync(author.Id).Returns(author);

            _authorRepository.Delete(author).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.DeleteAuthor(author.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            _authorRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.DeleteAuthor(int.MaxValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteAuthor_ReturnsBadRequest_WhenAuthorIsNotDeleted()
        {
            // Arrange
            var author = CreateFakeAuthor("John", "Test");
            _authorRepository.GetByIdAsync(author.Id).Returns(author);

            _authorRepository.Delete(Arg.Any<Author>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.DeleteAuthor(author.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsNoContent_WhenAuthorIsUpdated()
        {
            // Arrange
            var author = CreateFakeAuthor("John", "Test");

            _authorRepository.GetByIdAsync(author.Id).Returns(author);

            var authorDTO = new AuthorUpdateDTO
            {
                FirstName = "John",
                LastName = "Updated"
            };

            _authorRepository.Update(Arg.Any<Author>()).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.UpdateAuthor(author.Id, authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var authorDTO = new AuthorUpdateDTO
            {
                FirstName = "John",
                LastName = "Test"
            };

            _authorRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.UpdateAuthor(int.MaxValue, authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenAuthorIsNotUpdated()
        {
            // Arrange
            var author = CreateFakeAuthor("John", "Test");
            _authorRepository.GetByIdAsync(author.Id).Returns(author);

            var authorDTO = new AuthorUpdateDTO
            {
                FirstName = "John",
                LastName = "Updated"
            };

            _authorRepository.Update(Arg.Any<Author>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.UpdateAuthor(author.Id, authorDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}
