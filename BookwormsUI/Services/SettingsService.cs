using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IConfiguration _config;
        public SettingsService(IConfiguration config)
        {
            _config = config;
        }

        public ApiEndpoints GetAppSettingsApiEndpoints()
        {
            return _config.GetSection("Endpoints").Get<ApiEndpoints>();
        }

        public string GetAssetBaseUrl()
        {
            return _config.GetValue<string>("AssetBaseUrl");
        }

        public string GetApiBaseUrl()
        {
            var endpoints = _config.GetSection("Endpoints").Get<ApiEndpoints>();
            return endpoints.BaseUrl;
        }
    }
}