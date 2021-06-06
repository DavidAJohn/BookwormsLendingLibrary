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
        private readonly HttpClient _httpClient;
        private readonly SettingsService _settings;
        public AdminService(HttpClient httpClient, ILocalStorageService localStorage, SettingsService settings)
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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            var response = await _httpClient.GetAsync(_settings.GetApiBaseUrl() + "/admin/status");

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