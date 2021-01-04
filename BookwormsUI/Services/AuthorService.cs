namespace BookwormsUI.Services
{
    public class AuthorService
    {
        private readonly SettingsService _settings;
        public AuthorService(SettingsService settings)
        {
            _settings = settings;
        }

        public string GetAuthorsApiEndpoint()
        {
            var settings = _settings.GetAppSettingsApiEndpoints();

            var baseUrl = settings.BaseUrl;
            var authorsEndpoint = settings.AuthorsEndpoint;
            var url = baseUrl + authorsEndpoint;

            return url;
        }
    }
}