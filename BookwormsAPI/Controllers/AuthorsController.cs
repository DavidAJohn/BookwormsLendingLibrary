using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class AuthorsController : BaseApiController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors([FromQuery] AuthorSpecificationParams authorParams)
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
        public async Task<ActionResult> GetAuthorById(int id)
        {
            var spec = new AuthorsWithBooksSpecification(id);
            var author = await _authorRepository.GetEntityWithSpec(spec);

            if (author == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Author, AuthorDTO>(author));
        }

        // POST api/authors
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthorDTO>> CreateAuthor([FromBody] AuthorCreateDTO authorCreateDTO)
        {
            if (authorCreateDTO == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var author = _mapper.Map<Author>(authorCreateDTO);
            await _authorRepository.Create(author);
            var authorDTO = _mapper.Map<AuthorDTO>(author);

            return CreatedAtRoute(nameof(GetAuthorById), new {Id = authorDTO.Id}, authorDTO);
        }

        // DELETE api/authors/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new ApiResponse(404));
            }

            await _authorRepository.Delete(author);
            return NoContent();
        }

        // PUT api/authors/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateAuthor(int id, [FromBody] AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new ApiResponse(404));
            }

            _mapper.Map(authorUpdateDTO, author);
            await _authorRepository.Update(author);

            return NoContent();
        }

        // PATCH api/authors/{id}
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PatchAuthor(int id, JsonPatchDocument<AuthorUpdateDTO> patchDocument)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var authorToPatch = _mapper.Map<AuthorUpdateDTO>(author);
            patchDocument.ApplyTo(authorToPatch, ModelState);

            if (!TryValidateModel(authorToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(authorToPatch, author);
            await _authorRepository.Update(author);
            
            return NoContent();
        }
    }
}