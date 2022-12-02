using BookwormsUI.Models;

namespace BookwormsUI.Contracts;

public interface IAuthorService : IBaseService<Author>
{
    string GetAuthorsApiEndpoint();
}
