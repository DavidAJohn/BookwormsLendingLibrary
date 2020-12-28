using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    public class BooksController : BaseApiController
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BooksController(IBookRepository bookRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks([FromQuery] BookSpecificationParams bookParams)
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification(bookParams);

            // get any overall count of items
            var countSpec = new BooksWithFiltersForCountSpecification(bookParams);
            var totalItems = await _bookRepository.CountAsync(countSpec);

            // add pagination response headers to help client applications
            _httpContextAccessor.HttpContext.AddPaginationResponseHeaders(totalItems, bookParams.PageSize, bookParams.PageIndex);

            var books = await _bookRepository.ListAsync(spec);
            
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(books));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> GetBookById(int id)
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification(id);
            var book = await _bookRepository.GetEntityWithSpec(spec);

            if (book == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<Book, BookDTO>(book);
        }
    }
}