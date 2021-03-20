using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BookwormsAPI.Entities.Borrowing
{
    public class Request : BaseEntity
    {
        public Request()
        {
        }

        public Request(int bookId, string borrowerEmail, Address sendToAddress)
        {
            BorrowerEmail = borrowerEmail;
            SendToAddress = sendToAddress;
            BookId = bookId;
        }

        [Required]
        public string BorrowerEmail { get; set; }
        [Required]
        public Address SendToAddress { get; set; }
        [Required]
        public int BookId { get; set; }
        public Book Book { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTimeOffset DateRequested { get; set; } = DateTimeOffset.Now;
        public DateTime? DateSent { get; set; } = null;
        public DateTime? DateDue { get; set; } = null;
        public DateTime? DateReturned { get; set; } = null;

        public DateTime GetDateDue(DateTime dateSent)
        {
            return dateSent.AddDays(28);
        }
    }
}