using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookwormsAPI.Contracts;
using BookwormsAPI.DTOs;
using BookwormsAPI.Entities.Borrowing;
using BookwormsAPI.Errors;
using BookwormsAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    [Authorize]
    public class RequestsController : BaseApiController
    {
        private readonly IRequestService _requestService;
        private readonly IMapper _mapper;
        public RequestsController(IRequestService requestService, IMapper mapper)
        {
            _mapper = mapper;
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<ActionResult<Request>> CreateRequestAsync(RequestDTO requestDTO)
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();
            var address = _mapper.Map<AddressDTO, Address>(requestDTO.SendToAddress);

            var request = await _requestService.CreateRequestAsync(requestDTO.BookId, email, address);

            if (request == null)
            {
                return BadRequest(new ApiResponse(400, "There was a problem creating this book request"));
            }

            if (request.BorrowerEmail != email)
            {
                return BadRequest(new ApiResponse(400, "You already have an outstanding request for this book"));
            }

            return Ok(request);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestToReturnDTO>>> GetRequestsForUserAsync()
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();
            var requests = await _requestService.GetRequestsForUserAsync(email);
            return Ok(_mapper.Map<IEnumerable<Request>, IEnumerable<RequestToReturnDTO>>(requests));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<RequestToReturnDTO>>> GetRequestByIdAsync(int id)
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();
            var request = await _requestService.GetRequestByIdAsync(id, email);

            if (request == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Request, RequestToReturnDTO>(request));
        }

    }
}