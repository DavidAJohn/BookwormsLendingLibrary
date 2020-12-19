using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.Data;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BooksController(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookToReturnDTO>>> GetBooks()
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification();
            var books = await _bookRepository.ListAsync(spec);
            
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookToReturnDTO>>(books));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookToReturnDTO>> GetBookById(int id)
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification(id);
            var book = await _bookRepository.GetEntityWithSpec(spec);

            return _mapper.Map<Book, BookToReturnDTO>(book);
        }
    }
}