namespace BookwormsUI.Services
{
    public class BookService
    {
        private readonly SettingsService _settings;
        public BookService(SettingsService settings)
        {
            _settings = settings;
        }

        public string GetBooksApiEndpoint()
        {
            var settings = _settings.GetAppSettingsApiEndpoints();

            var baseUrl = settings.BaseUrl;
            var booksEndpoint = settings.BooksEndpoint;
            var url = baseUrl + booksEndpoint;

            return url;
        }
    }
}