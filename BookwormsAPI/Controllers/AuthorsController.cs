using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    public class AuthorsController : BaseApiController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<AuthorsController> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        // GET api/authors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAuthors([FromQuery] AuthorSpecificationParams authorParams)
        {
            var spec = new AuthorsWithBooksSpecification(authorParams);

            // get any overall count of items (after filtering has been applied)
            var countSpec = new AuthorsWithFiltersForCountSpecification(authorParams);
            var totalItems = await _authorRepository.CountAsync(countSpec);

            // add pagination response headers to help client applications
            _httpContextAccessor.HttpContext.AddPaginationResponseHeaders(totalItems, authorParams.PageSize, authorParams.PageIndex);

            var authors = await _authorRepository.ListAsync(spec);
            
            return Ok(_mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDTO>>(authors));
        }

        // GET api/authors/{id}
        [HttpGet("{id}", Name="GetAuthorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var spec = new AuthorsWithBooksSpecification(id);
            var author = await _authorRepository.GetEntityWithSpec(spec);

            if (author == null)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} was not found during GET request", id);
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Author, AuthorDTO>(author));
        }

        // POST api/authors
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            if (authorCreateDTO == null)
            {
                _logger.LogInformation("Authors Controller -> AuthorCreateDTO was null");
                return BadRequest(new ApiResponse(400));
            }

            var author = _mapper.Map<Author>(authorCreateDTO);

            var created = await _authorRepository.Create(author);

            if (created == null)
            {
                _logger.LogInformation("Authors Controller -> Author could not be created");
                return BadRequest(new ApiResponse(400));
            }

            var authorDTO = _mapper.Map<AuthorDTO>(author);

            return CreatedAtRoute(nameof(GetAuthorById), new {Id = authorDTO.Id}, authorDTO);
        }

        // DELETE api/authors/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} was not found during DELETE request", id);
                return NotFound(new ApiResponse(404));
            }

            var deleted = await _authorRepository.Delete(author);

            if (!deleted)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} could not be deleted", id);
                return BadRequest(new ApiResponse(400, "There was a problem deleting this author"));
            }

            return NoContent();
        }

        // PUT api/authors/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} was not found during PUT request", id);
                return NotFound(new ApiResponse(404));
            }

            _mapper.Map(authorUpdateDTO, author);

            var updated = await _authorRepository.Update(author);

            if (!updated)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} could not be updated", id);
                return BadRequest(new ApiResponse(400, "There was a problem updating this author"));
            }

            return NoContent();
        }

        // PATCH api/authors/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PatchAuthor(int id, JsonPatchDocument<AuthorUpdateDTO> patchDocument)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} was not found during PATCH request", id);
                return NotFound(new ApiResponse(404));
            }

            var authorToPatch = _mapper.Map<AuthorUpdateDTO>(author);
            patchDocument.ApplyTo(authorToPatch, ModelState);

            if (!TryValidateModel(authorToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(authorToPatch, author);

            var updated = await _authorRepository.Update(author);

            if (!updated)
            {
                _logger.LogInformation("Authors Controller -> Author with id: {id} could not be updated", id);
                return BadRequest(new ApiResponse(400, "There was a problem updating this author"));
            }

            return NoContent();
        }
    }
}