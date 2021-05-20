using System;
using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.Entities
{
    public class Book : BaseEntity
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public int YearPublished { get; set; }

        public string ISBN { get; set; }
        
        [Required]
        public string Summary { get; set; }

        public string CoverImageUrl { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int Copies { get; set; } = 1;

        public int RequestCount { get; set; } = 0;

        public DateTime AddedOn { get; set; } = DateTime.Now;

        public bool isActive { get; set; } = true;
    }
}