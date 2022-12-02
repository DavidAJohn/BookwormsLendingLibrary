using BookwormsUI.Models;

namespace BookwormsUI.Contracts
{
    public interface IBookService : IBaseService<Book>
    {
        string GetBooksApiEndpoint();
    }
}
