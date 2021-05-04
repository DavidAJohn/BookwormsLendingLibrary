using BookwormsUI.Models;
using Microsoft.Extensions.Configuration;

namespace BookwormsUI.Services
{
    public class SettingsService
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
    }
}