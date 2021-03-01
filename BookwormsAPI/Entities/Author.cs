using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.Entities
{
    public class Author : BaseEntity
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}