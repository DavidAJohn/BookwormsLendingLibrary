using System;

namespace BookwormsAPI.Entities.Borrowing
{
    public class Request : BaseEntity
    {
        public Request()
        {
        }

        public Request(int requestedBookId, string borrowerEmail, Address sendToAddress)
        {
            BorrowerEmail = borrowerEmail;
            SendToAddress = sendToAddress;
            RequestedBookId = requestedBookId;
        }

        public string BorrowerEmail { get; set; }
        public Address SendToAddress { get; set; }
        public int RequestedBookId { get; set; }
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