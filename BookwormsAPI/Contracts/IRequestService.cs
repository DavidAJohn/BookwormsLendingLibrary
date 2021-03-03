using System.Collections.Generic;
using System.Threading.Tasks;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.Contracts
{
    public interface IRequestService
    {
        Task<Request> CreateRequestAsync(int requestedBookId, string borrowerEmail, Address sendToAddress);
        Task<IEnumerable<Request>> GetRequestsForUserAsync(string borrowerEmail);
        Task<Request> GetRequestByIdAsync(int id, string borrowerEmail);
        Task<IEnumerable<Request>> GetRequestsByStatusAsync(RequestStatus requestStatus);
    }
}