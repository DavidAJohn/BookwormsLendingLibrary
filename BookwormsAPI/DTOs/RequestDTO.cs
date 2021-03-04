namespace BookwormsAPI.DTOs
{
    public class RequestDTO
    {
        public int BookId { get; set; }
        public AddressDTO SendToAddress { get; set; }
    }
}