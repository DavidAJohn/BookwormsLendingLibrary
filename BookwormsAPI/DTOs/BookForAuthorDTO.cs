using BookwormsAPI.Entities;

namespace BookwormsAPI.DTOs
{
    public class BookForAuthorDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearPublished { get; set; }
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
    }
}