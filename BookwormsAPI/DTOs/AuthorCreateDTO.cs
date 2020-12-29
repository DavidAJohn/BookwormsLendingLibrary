using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.DTOs
{
    public class AuthorCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(250)]
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
    }
}