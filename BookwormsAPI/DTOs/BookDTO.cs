using System.Collections.Generic;
using BookwormsAPI.Entities;

namespace BookwormsAPI.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public string Author { get; set; }
        public int AuthorId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
    }
}