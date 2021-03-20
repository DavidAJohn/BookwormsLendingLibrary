using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BookwormsAPI.Entities.Borrowing
{
    [JsonConverter(typeof(JsonStringEnumConverter))]    
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
