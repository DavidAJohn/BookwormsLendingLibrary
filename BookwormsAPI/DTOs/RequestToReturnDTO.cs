using System;
using BookwormsAPI.Entities.Borrowing;

namespace BookwormsAPI.DTOs
{
    public class RequestToReturnDTO
    {
        public int Id { get; set; }
        public string BorrowerEmail { get; set; }
        public Address SendToAddress { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public string Status { get; set; }
        public DateTimeOffset DateRequested { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateDue { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}