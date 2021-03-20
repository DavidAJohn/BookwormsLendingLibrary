using System.ComponentModel.DataAnnotations;

namespace BookwormsUI.Models
{
    public class RequestUpdate
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public RequestStatus Status { get; set; }
    }
}