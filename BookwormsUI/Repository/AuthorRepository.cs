using System.Net.Http;
using Blazored.LocalStorage;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        public AuthorRepository(IHttpClientFactory client, ILocalStorageService localStorage) : base(client, localStorage)
        {
            _localStorage = localStorage;
            _client = client;
        }
    }
}