using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace BookwormsUI.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        public BaseRepository(IHttpClientFactory client, ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _client = client;
        }

        public async Task<T> GetByIdAsync(string url, int id)
        {
            if (id < 0)
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url + "/" + id);

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                var json = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content);
                return json;
            }

            return null;
        }

        public async Task<PagedList<T>> GetAsync(string url, ItemParameters itemParams)
        {
            var request = new HttpRequestMessage();

            if (itemParams != null) 
            {
                var queryStringParams = new Dictionary<string, string>
                {
                    ["pageIndex"] = itemParams.PageIndex < 1  ? "1" : itemParams.PageIndex.ToString(),
                };

                // conditionally add a page size param (number of items to return)
                if (itemParams.PageSize != 0)
                {
                    queryStringParams.Add("pageSize", itemParams.PageSize.ToString());
                };

                // conditionally add a search term
                if (!String.IsNullOrEmpty(itemParams.Search))
                {
                    queryStringParams.Add("search", itemParams.Search.ToString());
                };

                // conditionally add a categoryId param
                if (itemParams.CategoryId != 0)
                {
                    queryStringParams.Add("categoryId", itemParams.CategoryId.ToString());
                };

                // conditionally add a sort param
                if (!String.IsNullOrEmpty(itemParams.SortBy))
                {
                    queryStringParams.Add("sort", itemParams.SortBy.ToString());
                };

                request = new HttpRequestMessage(HttpMethod.Get, QueryHelpers.AddQueryString(url, queryStringParams));
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, url);
            }
            
            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();

                var pagedResponse = new PagedList<T>
                {
                    Items = JsonSerializer.Deserialize<List<T>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                    Metadata = JsonSerializer.Deserialize<PagingMetadata>(
                        response.Headers.GetValues("Pagination")
                        .First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                };

                return pagedResponse;
            }

            return null;
        }

        public async Task<bool> CreateAsync(string url, T obj)
        {
            if (obj == null)
            {
                return false;
            }

            var storedToken = await GetLocalBearerToken();

            if (string.IsNullOrWhiteSpace(storedToken))
            {
                return false;
            }

            HttpClient client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);

            HttpContent content = new StringContent(JsonSerializer.Serialize(obj));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            if (id < 1)
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id);

            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateAsync(string url, T obj, int id)
        {
            if (obj == null || id < 0)
            {
                return false;
            }

            var storedToken = await GetLocalBearerToken();

            if (string.IsNullOrWhiteSpace(storedToken))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Put, url + "/" + id);
            request.Content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");

            var client = _client.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", storedToken);
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }

            return false;
        }

        private async Task<string> GetLocalBearerToken()
        {
            return await _localStorage.GetItemAsync<string>("authToken");
        }
    }
}