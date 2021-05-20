using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public bool isActive { get; set; }
    }
}