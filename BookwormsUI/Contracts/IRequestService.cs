using BookwormsUI.Models;

namespace BookwormsUI.Contracts;

public interface IRequestService
{
    string GetApiEndpoint(string endpoint);
    Task<Address> GetBorrowerAddressAsync();
    Task<Address> SaveBorrowerAddressAsync(Address address);
    Task<RequestResult> CreateBookRequestAsync(int bookId, Address address);
    Task<List<Request>> GetRequestsForUserAsync();
    Task<List<Request>> GetRequestsByStatusAsync(RequestStatus status);
    Task<RequestResult> UpdateRequestStatusAsync(int requestId, RequestStatus newStatus);
    Task<List<Request>> GetOverdueRequestsAsync();
}
