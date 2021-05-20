using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookwormsAPI.Entities
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public bool isActive { get; set; } = true;
    }
}