using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;
using BookwormsUI.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookwormsUI.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        private readonly SettingsService _settings;
        public AuthenticationService(HttpClient httpClient, 
                            AuthenticationStateProvider authenticationStateProvider, 
                            ILocalStorageService localStorage,
                            SettingsService settings)
        {
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<RegisterResult> Register(RegisterModel registerModel)
        {
            JsonContent content = JsonContent.Create(registerModel);
            var response = await _httpClient.PostAsync(GetApiEndpoint("register"), content);

            if (response.IsSuccessStatusCode)
            {
                return new RegisterResult
                {
                    Successful = true,
                    Errors = new string[] {}
                };
            }

            return new RegisterResult
            {
                Successful = false,
                Errors = new string[] {response.StatusCode.ToString()}
            };
        }

        public async Task<LoginResult> Login(LoginModel loginModel)
        {
            JsonContent content = JsonContent.Create(loginModel);
            var response = await _httpClient.PostAsync(GetApiEndpoint("login"), content);

            var loginResult = JsonSerializer.Deserialize<LoginResult>(
                await response.Content.ReadAsStringAsync(), 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!response.IsSuccessStatusCode)
            {
                loginResult.Successful = false;
                loginResult.Error = "Invalid username or password";
                return loginResult;
            }

            loginResult.Successful = true;

            await _localStorage.SetItemAsync("authToken", loginResult.Token);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            return loginResult;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private string GetApiEndpoint(string endpoint)
        {
            var settings = _settings.GetAppSettingsApiEndpoints();
            var baseUrl = settings.BaseUrl;

            return endpoint switch
            {
                "register" => baseUrl + settings.RegisterEndpoint,
                "login" => baseUrl + settings.LoginEndpoint,
                _ => ""
            };
        }
    }
}