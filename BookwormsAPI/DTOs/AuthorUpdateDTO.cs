using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.DTOs
{
    public class AuthorUpdateDTO
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(500)]
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
        public bool isActive { get; set; }
    }
}