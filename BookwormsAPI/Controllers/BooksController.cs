using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        // GET api/books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBooks([FromQuery] BookSpecificationParams bookParams)
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification(bookParams);

            // get any overall count of items (after filtering has been applied)
            var countSpec = new BooksWithFiltersForCountSpecification(bookParams);
            var totalItems = await _bookRepository.CountAsync(countSpec);

            // add pagination response headers to help client applications
            _httpContextAccessor.HttpContext.AddPaginationResponseHeaders(totalItems, bookParams.PageSize, bookParams.PageIndex);

            var books = await _bookRepository.ListAsync(spec);
            
            return Ok(_mapper.Map<IEnumerable<Book>, IEnumerable<BookDTO>>(books));
        }

        // GET api/books/{id}
        [HttpGet("{id}", Name="GetBookById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int id)
        {
            var spec = new BooksWithCategoriesAndAuthorsSpecification(id);
            var book = await _bookRepository.GetEntityWithSpec(spec);

            if (book == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Book, BookDTO>(book));
        }

        // POST api/books
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDTO bookCreateDTO)
        {
            if (bookCreateDTO == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            var book = _mapper.Map<Book>(bookCreateDTO);
            await _bookRepository.Create(book);
            var bookDTO = _mapper.Map<Book, BookForAuthorDTO>(book);

            return CreatedAtRoute(nameof(GetBookById), new {Id = bookDTO.Id}, bookDTO);
        }

        // DELETE api/books/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var deleted = await _bookRepository.Delete(book);

            if (!deleted)
            {
                return BadRequest(new ApiResponse(400, "There was a problem deleting this book"));
            }

            return NoContent();
        }

        // PUT api/books/{id}
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDTO bookUpdateDTO)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse(404));
            }

            _mapper.Map(bookUpdateDTO, book);
            
            var updated = await _bookRepository.Update(book);

            if (!updated)
            {
                return BadRequest(new ApiResponse(400, "There was a problem updating this book"));
            }

            return NoContent();
        }

        // PATCH api/books/{id}
        [HttpPatch("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchBook(int id, JsonPatchDocument<BookUpdateDTO> patchDocument)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            if (book == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var bookToPatch = _mapper.Map<BookUpdateDTO>(book);
            patchDocument.ApplyTo(bookToPatch, ModelState);

            if (!TryValidateModel(bookToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(bookToPatch, book);

            var updated = await _bookRepository.Update(book);

            if (!updated)
            {
                return BadRequest(new ApiResponse(400, "There was a problem updating this book"));
            }

            return NoContent();
        }
    }
}