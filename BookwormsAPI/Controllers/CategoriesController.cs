using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET api/categories
        [HttpGet]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var spec = new CategoriesOrderedByNameSpecification();
            var categories = await _categoryRepository.ListAsync(spec);

            return Ok(categories);
        }

        // GET api/categories/{id}
        [HttpGet("{id}", Name="GetCategoryById")]
        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] {"id"})]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCategoryById(int id)
        {
            var spec = new CategoriesOrderedByNameSpecification(id);
            var category = await _categoryRepository.GetEntityWithSpec(spec);

            if (category == null) 
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(category);
        }

        // POST api/categories
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
        {
            if (categoryCreateDTO == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var category = _mapper.Map<Category>(categoryCreateDTO);

            var created = await _categoryRepository.Create(category);

            if (created == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return CreatedAtRoute(nameof(GetCategoryById), new {Id = categoryDTO.Id}, categoryDTO);
        }

        // DELETE api/categories/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var deleted = await _categoryRepository.Delete(category);

            if (!deleted)
            {
                return BadRequest(new ApiResponse(400, "There was a problem deleting this category"));
            }

            return NoContent();
        }

        // PUT api/categories/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateCategory(int id, CategoryCreateDTO categoryUpdateDTO)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse(404));
            }

            _mapper.Map(categoryUpdateDTO, category);

            var updated = await _categoryRepository.Update(category);

            if (!updated)
            {
                return BadRequest(new ApiResponse(400, "There was a problem updating this category"));
            }

            return NoContent();
        }
    }
}