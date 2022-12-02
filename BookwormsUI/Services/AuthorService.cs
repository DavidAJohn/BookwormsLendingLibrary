using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Services;

public class AuthorService : BaseService<Author>, IAuthorService
{
    private readonly SettingsService _settings;

    public AuthorService(IHttpClientFactory client, ILocalStorageService localStorage, SettingsService settings) 
        : base(client, localStorage)
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
