namespace BookwormsUI.Models
{
    public class Request
    {
        public int BookId { get; set; }
        public Address SendToAddress { get; set; }
    }
}