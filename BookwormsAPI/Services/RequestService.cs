using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Entities.Borrowing;
using BookwormsAPI.Specifications;

namespace BookwormsAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }

        private DateTime GetDateDue(DateTime dateSent)
        {
            return dateSent.AddDays(28); // hard-coding a 28-day loan period?
        }

        public async Task<Request> CreateRequestAsync(int bookId, string borrowerEmail, Address sendToAddress)
        {
            if (bookId <= 0) return null;

            // check if a pending request for the book already exists,
            // or if the borrower already has an overdue copy
            var existingSpec = new RequestOutstandingSpecification(bookId, borrowerEmail);
            var existingRequest = await _requestRepository.GetEntityWithSpec(existingSpec);

            if (existingRequest != null)
            {
                existingRequest.BorrowerEmail = "outstanding.request@test.com";
                return existingRequest;
            }

            // if not, create a new one
            var request = new Request(bookId, borrowerEmail, sendToAddress);
            var result = await _requestRepository.Create(request);

            if (result == null) return null;

            return result;
        }

        public async Task<IEnumerable<Request>> GetRequestsForUserAsync(string borrowerEmail)
        {
            var spec = new RequestsForUserWithBookDetailsSpecification(borrowerEmail);
            return await _requestRepository.ListAsync(spec);
        }

        public async Task<Request> GetRequestByIdAsync(int id)
        {
            var spec = new RequestByIdWithBookDetailsSpecification(id);
            return await _requestRepository.GetEntityWithSpec(spec);
        }

        public async Task<IEnumerable<Request>> GetRequestsByStatusAsync(RequestStatus requestStatus)
        {
            var spec = new RequestsByStatusWithBookDetailsSpecification(requestStatus);
            return await _requestRepository.ListAsync(spec);
        }

        public async Task<IEnumerable<Request>> GetRequestsOverdueAsync()
        {
            var spec = new RequestsOverdueSpecification();
            return await _requestRepository.ListAsync(spec);
        }

        public async Task<Request> UpdateRequestStatusAsync(Request request, RequestStatus newRequestStatus)
        {
            if (request == null) return null;

            // these parts of the request are staying the same
            var requestUpdate = new Request {
                Id = request.Id,
                BorrowerEmail = request.BorrowerEmail,
                SendToAddress = request.SendToAddress,
                BookId = request.BookId,
                Book = request.Book,
                DateRequested = request.DateRequested
            };

            // these parts depend on the status the request is being changed to
            switch (newRequestStatus)
            {
                case RequestStatus.Sent:
                    requestUpdate.Status = RequestStatus.Sent;
                    requestUpdate.DateSent = DateTime.Now;
                    requestUpdate.DateDue = GetDateDue(DateTime.Now); // set the date due to n days from now
                    break;

                case RequestStatus.Returned:
                    requestUpdate.DateSent = request.DateSent;
                    requestUpdate.Status = RequestStatus.Returned;
                    requestUpdate.DateReturned = DateTime.Now;
                    break;

                case RequestStatus.Cancelled:
                    requestUpdate.Status = RequestStatus.Cancelled;
                    break;

                default:
                    break;
            };

            if (requestUpdate == null) return null;

            var updateResponse = await _requestRepository.Update(requestUpdate);

            if (updateResponse)
            {
                return requestUpdate;
            }

            return null;
        }
    }
}