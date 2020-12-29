using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.DTOs
{
    public class BookCreateDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
        
        [Required]
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }
}