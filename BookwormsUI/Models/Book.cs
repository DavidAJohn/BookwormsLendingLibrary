using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class Book
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public int YearPublished { get; set; }
        public string ISBN { get; set; }
        
        [Required]
        public string Summary { get; set; }
        public string CoverImageUrl { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}