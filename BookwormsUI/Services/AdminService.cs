using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;
using Newtonsoft.Json;

namespace BookwormsUI.Services
{
    public class AdminService : IAdminService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpClientFactory _httpClient;
        private readonly ISettingsService _settings;
        public AdminService(IHttpClientFactory httpClient, ILocalStorageService localStorage, ISettingsService settings)
        {
            _settings = settings;
            _httpClient = httpClient;
            _localStorage = localStorage;
        }

        public async Task<SiteStatusTotals> GetSiteStatusAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return null;
            }

            var client = _httpClient.CreateClient("BookwormsAPI");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            var response = await client.GetAsync(_settings.GetApiBaseUrl() + "/admin/status");

            if (response.IsSuccessStatusCode)
            {
                var status = JsonConvert.DeserializeObject<SiteStatusTotals>(
                    await response.Content.ReadAsStringAsync()
                );

                return status;
            }

            return null;
        }
    }
}