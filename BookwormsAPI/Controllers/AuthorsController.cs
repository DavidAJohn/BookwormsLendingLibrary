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

        [HttpGet("{id}")]
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
    }
}