using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var spec = new CategoriesOrderedByNameSpecification();
            var categories = await _categoryRepository.ListAsync(spec);

            return Ok(categories);
        }

        [HttpGet("{id}")]
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
    }
}