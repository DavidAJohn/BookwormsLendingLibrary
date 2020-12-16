using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.Entities
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}