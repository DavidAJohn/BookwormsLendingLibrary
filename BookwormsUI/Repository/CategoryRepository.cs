using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Repository
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        public CategoryRepository(IHttpClientFactory client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            _localStorage = localStorage;
            _client = client;
        }

        public async Task<List<Category>> GetListAsync(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var client = _client.CreateClient();
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
}