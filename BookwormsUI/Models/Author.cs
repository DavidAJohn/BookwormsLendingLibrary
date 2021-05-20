using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class Author
    {
        public int Id { get; set; }
        
        [Required]
        [DisplayName("First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [StringLength(500)]
        public string Biography { get; set; }
        public string AuthorImageUrl { get; set; }
        public ICollection<Book> Books { get; set; }
        public bool isActive { get; set; }
    }
}