using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Tests.UnitTests.Controllers
{
    public class CategoriesControllerTests : TestBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private CategoriesController _sut;

        public CategoriesControllerTests()
        {
            _mapper = BuildMap();
            _categoryRepository = Substitute.For<ICategoryRepository>();

            _sut = new CategoriesController(_categoryRepository, _mapper);
        }

        [Fact]
        public async Task GetCategories_ReturnsListOfCategories_WhenCalled()
        {
            // Arrange
            _categoryRepository
                .ListAsync(Arg.Any<CategoriesOrderedByNameSpecification>())
                .Returns(new List<Category>());

            // Act
            var result = (OkObjectResult)await _sut.GetCategories();

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<List<Category>>();
        }

        [Fact]
        public async Task GetCategoryById_ReturnsCategory_WhenIdExists()
        {
            // Arrange
            var id = 1;

            Category category = new()
            {
                Id = id,
                Name = "Category 1"
            };

            _categoryRepository
                .GetEntityWithSpec(Arg.Any<CategoriesOrderedByNameSpecification>())
                .Returns(category);

            // Act
            var result = (OkObjectResult)await _sut.GetCategoryById(id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeOfType<Category>();
            result.Value.Should().BeEquivalentTo(category);
        }

        [Fact]
        public async Task GetCategoryById_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            _categoryRepository
                .GetEntityWithSpec(Arg.Any<CategoriesOrderedByNameSpecification>())
                .ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.GetCategoryById(int.MaxValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task CreateCategory_ReturnsCategoryDTO_WhenCategoryIsCreated()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            var category = new Category
            {
                Id = 1,
                Name = "Category 1"
            };

            _categoryRepository.Create(Arg.Any<Category>()).Returns(category);

            // Act
            var result = (CreatedAtRouteResult)await _sut.CreateCategory(catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.RouteName.Should().Be("GetCategoryById");
            result.Value.Should().BeOfType<CategoryDTO>();
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenInputIsNull()
        {
            // Arrange
            CategoryCreateDTO catDTO = null;

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateCategory(catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenInputIsInvalid()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = null
            };

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateCategory(catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateCategory_ReturnsBadRequest_WhenCategoryCouldNotBeCreated()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            _categoryRepository.Create(Arg.Any<Category>()).ReturnsNull();

            // Act
            var result = (BadRequestObjectResult)await _sut.CreateCategory(catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNoContent_WhenCategoryIsDeleted()
        {
            // Arrange
            var id = 1;

            Category category = new()
            {
                Id = id,
                Name = "Category 1"
            };

            _categoryRepository.GetByIdAsync(Arg.Any<int>()).Returns(category);
            _categoryRepository.Delete(Arg.Any<Category>()).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.DeleteCategory(id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            _categoryRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.DeleteCategory(int.MinValue);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsBadRequest_WhenCategoryIsNotDeleted()
        {
            // Arrange
            Category category = new()
            {
                Id = 1,
                Name = "Category 1"
            };

            _categoryRepository.GetByIdAsync(category.Id).Returns(category);

            _categoryRepository.Delete(Arg.Any<Category>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.DeleteCategory(category.Id);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNoContent_WhenCategoryIsUpdated()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            var category = new Category
            {
                Id = 1,
                Name = "Category 1"
            };

            _categoryRepository.GetByIdAsync(category.Id).Returns(category);
            _categoryRepository.Update(Arg.Any<Category>()).Returns(true);

            // Act
            var result = (NoContentResult)await _sut.UpdateCategory(category.Id, catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            _categoryRepository.GetByIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act
            var result = (NotFoundObjectResult)await _sut.UpdateCategory(int.MaxValue, catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateCategory_ReturnsBadRequest_WhenCategoryWasNotUpdated()
        {
            // Arrange
            var catDTO = new CategoryCreateDTO
            {
                Name = "Category 1"
            };

            var category = new Category
            {
                Id = 1,
                Name = "Category 1"
            };

            _categoryRepository.GetByIdAsync(category.Id).Returns(category);
            _categoryRepository.Update(Arg.Any<Category>()).Returns(false);

            // Act
            var result = (BadRequestObjectResult)await _sut.UpdateCategory(category.Id, catDTO);

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }
    }
}