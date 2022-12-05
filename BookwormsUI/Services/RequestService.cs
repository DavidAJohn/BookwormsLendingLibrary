using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BookwormsUI.Services;

public class RequestService : IRequestService
{
    private readonly SettingsService _settings;
    private readonly IHttpClientFactory _client;
    private readonly ILocalStorageService _localStorage;
    public RequestService(SettingsService settings, IHttpClientFactory client, ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _client = client;
        _settings = settings;
    }

    public string GetApiEndpoint(string endpoint)
    {
        var settings = _settings.GetAppSettingsApiEndpoints();
        var baseUrl = settings.BaseUrl;

        return endpoint.ToLower() switch
        {
            "address" => baseUrl + settings.AddressEndpoint,
            "requests" => baseUrl + settings.RequestsEndpoint,
            _ => null
        };
    }

    public async Task<Address> GetBorrowerAddressAsync() 
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new Address {};
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        HttpResponseMessage response = await client.GetAsync(GetApiEndpoint("address"));

        if (response.IsSuccessStatusCode)
        {
            var address = JsonConvert.DeserializeObject<Address>(
                await response.Content.ReadAsStringAsync()
            );

            return address;
        }

        return null;
    }
    

    public async Task<Address> SaveBorrowerAddressAsync(Address address)
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        string json = JsonConvert.SerializeObject(address);
        StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PutAsync(GetApiEndpoint("address"), httpContent);

        if (response.IsSuccessStatusCode)
        {
            var newAddress = JsonConvert.DeserializeObject<Address>(
                await response.Content.ReadAsStringAsync()
            );

            return newAddress;
        }

        return null;
    }

    public async Task<RequestResult> CreateBookRequestAsync(int bookId, Address address)
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var bookRequest = new RequestCreate {
            BookId = bookId,
            SendToAddress = new Address {
                FirstName = address.FirstName,
                LastName = address.LastName,
                Street = address.Street,
                City = address.City,
                County = address.County,
                PostCode = address.PostCode             
            }
        };

        string json = JsonConvert.SerializeObject(bookRequest);
        StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PostAsync(GetApiEndpoint("requests"), httpContent);

        if (response.IsSuccessStatusCode)
        {
            return new RequestResult {
                Successful = true,
                Error = ""
            };
        }

        var error = JsonConvert.DeserializeObject<ApiErrorResponse>(
            await response.Content.ReadAsStringAsync()
        );

        return new RequestResult {
            Successful = false,
            Error = error.Message
        };
    }

    public async Task<List<Request>> GetRequestsForUserAsync() 
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var response = await client.GetAsync(GetApiEndpoint("requests"));

        if (response.IsSuccessStatusCode)
        {
            var requests = JsonConvert.DeserializeObject<List<Request>>(
                await response.Content.ReadAsStringAsync()
            );

            return requests;
        }

        return null;
    }

    public async Task<List<Request>> GetRequestsByStatusAsync(RequestStatus status) 
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var response = await client.GetAsync(GetApiEndpoint("requests") + "/status?status=" + status);

        if (response.IsSuccessStatusCode)
        {
            var requests = JsonConvert.DeserializeObject<List<Request>>(
                await response.Content.ReadAsStringAsync()
            );

            return requests;
        }

        return null;
    }

    public async Task<RequestResult> UpdateRequestStatusAsync(int requestId, RequestStatus newStatus)
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var requestUpdate = new RequestUpdate {
            Id = requestId,
            Status = newStatus
        };

        string json = JsonConvert.SerializeObject(requestUpdate);
        StringContent httpContent = new(json, System.Text.Encoding.UTF8, "application/json");

        var response = await client.PutAsync(GetApiEndpoint("requests") + "/" + requestId, httpContent);

        if (response.IsSuccessStatusCode)
        {
            return new RequestResult {
                Successful = true,
                Error = ""
            };
        }
        else
        {
            return new RequestResult {
                Successful = false,
                Error = "This request could not be updated"
            };
        }
        
    }

    public async Task<List<Request>> GetOverdueRequestsAsync() 
    {
        var savedToken = await _localStorage.GetItemAsync<string>("authToken");

        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return null;
        }

        var client = _client.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

        var response = await client.GetAsync(GetApiEndpoint("requests") + "/overdue");
        if (response.IsSuccessStatusCode)
        {
            var requests = JsonConvert.DeserializeObject<List<Request>>(
                await response.Content.ReadAsStringAsync()
            );

            return requests;
        }

        return null;
    }
}