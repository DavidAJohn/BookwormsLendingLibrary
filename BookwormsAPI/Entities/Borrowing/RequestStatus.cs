using System.Runtime.Serialization;

namespace BookwormsAPI.Entities.Borrowing
{
    public enum RequestStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,

        [EnumMember(Value = "Request Cancelled")]
        Cancelled,
        
        [EnumMember(Value = "Sent to Borrower")]
        Sent,
        
        [EnumMember(Value = "Returned by Borrower")]
        Returned
    }
}
