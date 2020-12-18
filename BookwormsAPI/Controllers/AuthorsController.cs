using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class AuthorsController : BaseApiController
    {
        private readonly IAuthorRepository _authorRepository;
        public AuthorsController(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            var spec = new AuthorsWithBooksSpecification();
            var authors = await _authorRepository.ListAsync(spec);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorById(int id)
        {
            var spec = new AuthorsWithBooksSpecification(id);
            var author = await _authorRepository.GetEntityWithSpec(spec);
            return Ok(author);
        }
    }
}