using System.Net.Http;
using BookwormsUI.Contracts;
using BookwormsUI.Models;

namespace BookwormsUI.Repository
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly IHttpClientFactory _client;
        public BookRepository(IHttpClientFactory client) : base(client)
        {
            _client = client;
        }
    }
}