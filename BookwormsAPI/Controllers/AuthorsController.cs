using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
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
        public AuthorsController(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> GetAuthors()
        {
            var spec = new AuthorsWithBooksSpecification();
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