namespace BookwormsUI.Models
{
    public class RequestCreate
    {
        public int BookId { get; set; }
        public Address SendToAddress { get; set; }
    }
}