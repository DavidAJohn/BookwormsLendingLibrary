using System.Net.Http;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Repository
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        private readonly IHttpClientFactory _client;
        public AuthorRepository(IHttpClientFactory client) : base(client)
        {
            _client = client;
        }
    }
}