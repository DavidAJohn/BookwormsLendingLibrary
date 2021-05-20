using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BookwormsAPI.Entities;

namespace BookwormsAPI.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [MaxLength(500)]
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
        public ICollection<BookForAuthorDTO> Books { get; set; }
    }
}