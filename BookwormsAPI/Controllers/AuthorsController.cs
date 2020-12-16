using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class AuthorsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Author>> GetAuthors()
        {
            var authors = await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();

            return authors;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books)
                .SingleOrDefaultAsync(a => a.Id == id);

            return Ok(author);
        }
    }
}