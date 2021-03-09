using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookwormsUI.Models;
using Newtonsoft.Json;

namespace BookwormsUI.Services
{
    public class RequestService
    {
        private readonly SettingsService _settings;
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public RequestService(SettingsService settings, HttpClient httpClient, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _settings = settings;
        }

        private string GetApiEndpoint(string endpoint)
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

        public async Task<Address> GetBorrowerAddress() 
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return new Address {};
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            var response = await _httpClient.GetAsync(GetApiEndpoint("address"));

            if (response.IsSuccessStatusCode)
            {
                var address = JsonConvert.DeserializeObject<Address>(
                    await response.Content.ReadAsStringAsync()
                );

                return address;
            }

            return null;
        }
        

        public async Task<Address> SaveBorrowerAddress(Address address)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            string json = JsonConvert.SerializeObject(address);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(GetApiEndpoint("address"), httpContent);

            if (response.IsSuccessStatusCode)
            {
                var newAddress = JsonConvert.DeserializeObject<Address>(
                    await response.Content.ReadAsStringAsync()
                );

                return newAddress;
            }

            return null;
        }

        public async Task<Request> CreateBookRequest(int bookId, Address address)
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            var bookRequest = new Request {
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
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(GetApiEndpoint("requests"), httpContent);

            if (response.IsSuccessStatusCode)
            {
                var newRequest = JsonConvert.DeserializeObject<Request>(
                    await response.Content.ReadAsStringAsync()
                );

                return newRequest;
            }

            return null;
        }
    }
}