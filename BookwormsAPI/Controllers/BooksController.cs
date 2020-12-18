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
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var spec = new BooksWithCategoriesSpecification();
            var books = await _bookRepository.ListAsync(spec);
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var spec = new BooksWithCategoriesSpecification(id);
            var book = await _bookRepository.GetEntityWithSpec(spec);
            return Ok(book);
        }
    }
}