using System.Linq;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Entities.Borrowing;
using BookwormsAPI.Entities.Identity;
using BookwormsAPI.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IRequestService _requestService;
        public AdminController(UserManager<AppUser> userManager, IBookRepository bookRepository, IAuthorRepository authorRepository, IRequestService requestService)
        {
            _requestService = requestService;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.Address)
                .OrderBy(u => u.UserName)
                .ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("status")]
        public async Task<ActionResult> GetSiteStatus()
        {
            var bookSpec = new BooksWithFiltersForCountSpecification(new BookSpecificationParams{});
            var bookTotal = await _bookRepository.CountAsync(bookSpec);

            var authorSpec = new AuthorsWithFiltersForCountSpecification(new AuthorSpecificationParams{});
            var authorTotal = await _authorRepository.CountAsync(authorSpec);

            var requestsOutstanding = await _requestService.GetRequestsByStatusAsync(RequestStatus.Sent);
            var requestsOverdue = await _requestService.GetRequestsOverdueAsync();
            
            return Ok(new{
                BookTotal = bookTotal,
                AuthorTotal = authorTotal,
                RequestsOutstanding = requestsOutstanding.Count(),
                RequestsOverdue = requestsOverdue.Count()
            });
        }
    }
}