using System;
using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class Request
    {
        [Required]
        public string BorrowerEmail { get; set; }
        [Required]
        public Address SendToAddress { get; set; }
        [Required]
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string Status { get; set; }
        public DateTimeOffset DateRequested { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateDue { get; set; }
    }
}