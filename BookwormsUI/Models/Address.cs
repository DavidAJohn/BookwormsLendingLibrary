using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        public string County { get; set; }

        [Required]
        public string PostCode { get; set; }
    }
}