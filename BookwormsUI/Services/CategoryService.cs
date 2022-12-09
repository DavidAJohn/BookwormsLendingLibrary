using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;
using System.Net;

namespace BookwormsUI.Services;

public class CategoryService : BaseService<Category>, ICategoryService
{
    private readonly IHttpClientFactory _client;
    private readonly SettingsService _settings;

    public CategoryService(IHttpClientFactory client, ILocalStorageService localStorage, SettingsService settings) 
        : base(client, localStorage)
    {
        _client = client;
        _settings = settings;
    }

    public string GetCategoriesApiEndpoint()
    {
        var settings = _settings.GetAppSettingsApiEndpoints();

        var baseUrl = settings.BaseUrl;
        var categoriesEndpoint = settings.CategoriesEndpoint;
        var url = baseUrl + categoriesEndpoint;

        return url;
    }

    public async Task<List<Category>> GetListAsync(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var client = _client.CreateClient("BookwormsAPI");
        HttpResponseMessage response = await client.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Category>>(content);
            return json;
        }

        return null;
    }
}
