using System.Collections.Generic;

namespace BookwormsAPI.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}