using System.Collections.Generic;

namespace BookwormsAPI.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}