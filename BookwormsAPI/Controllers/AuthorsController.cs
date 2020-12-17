using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.Entities;
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
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await _authorRepository.FindAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            // var author = await _context.Authors
            //     .Include(a => a.Books)
            //     .SingleOrDefaultAsync(a => a.Id == id);

            var author = await _authorRepository.FindByConditionAsync(x => x.Id == id);
            return Ok(author);
        }
    }
}