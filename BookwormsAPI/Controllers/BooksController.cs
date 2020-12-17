using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Book>> GetBooks()
        {
            // var books = await _context.Books
            //     .Include(b => b.Author)
            //     .Include(b => b.Category)
            //     .ToListAsync();

            return await _bookRepository.FindAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            // var book = await _context.Books
            //     .Include(b => b.Author)
            //     .Include(b => b.Category)
            //     .SingleOrDefaultAsync(b => b.Id == id);
            // return Ok(book);

            var book = await _bookRepository.FindByConditionAsync(x => x.Id == id);
            return Ok(book);
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateBook([FromBody] Book book) 
        // {
        //     var newBook = await _context.Books.AddAsync(book);
        //     await _context.SaveChangesAsync();
        //     return Ok(newBook);
        // }
    }
}