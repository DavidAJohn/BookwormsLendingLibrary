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
        private readonly IRequestRepository _requestRepository;
        public RequestsController(IRequestService requestService, IMapper mapper, IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
            _requestService = requestService;
        }

        [HttpPost]
        public async Task<ActionResult<Request>> CreateRequest(RequestDTO requestDTO)
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

            return CreatedAtRoute(nameof(GetRequestById), new { id = request.Id }, request);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestToReturnDTO>>> GetRequestsForUser()
        {
            var email = HttpContext.User.GetEmailFromClaimsPrincipal();
            var requests = await _requestService.GetRequestsForUserAsync(email);
            return Ok(_mapper.Map<IEnumerable<Request>, IEnumerable<RequestToReturnDTO>>(requests));
        }

        [HttpGet("{id}", Name = "GetRequestById")]
        public async Task<ActionResult<RequestToReturnDTO>> GetRequestById(int id)
        {
            var request = await _requestService.GetRequestByIdAsync(id);

            if (request == null) return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Request, RequestToReturnDTO>(request));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<RequestToReturnDTO>>> GetRequestsByStatus([FromQuery] RequestStatus status)
        {
            var requests = await _requestService.GetRequestsByStatusAsync(status);
            return Ok(_mapper.Map<IEnumerable<Request>, IEnumerable<RequestToReturnDTO>>(requests));
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRequestStatus(int id, [FromBody] NewStatus status)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            var updatedRequest = await _requestService.UpdateRequestStatusAsync(request, status.Status);

            if (updatedRequest == null) return BadRequest(new ApiResponse(400, "The specified book request could not be updated"));
            
            return NoContent();
        }
    }

    public class NewStatus
    {
        public RequestStatus Status { get; set; }
    }
}