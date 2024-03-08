using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Services
{
    public class BookService : BaseService<Book>, IBookService
    {
        private readonly ISettingsService _settings;

        public BookService(IHttpClientFactory client, ILocalStorageService localStorage, ISettingsService settings) 
            : base(client, localStorage)
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