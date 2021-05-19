namespace BookwormsUI.Models
{
    public class UploadResult
    {
        public string FileName { get; set; }
        public string Container { get; set; }
        public string Uri { get; set; }
        public int ErrorCode { get; set; }
    }
}