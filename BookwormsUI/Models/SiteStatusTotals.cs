namespace BookwormsUI.Models
{
    public class SiteStatusTotals
    {
        public int BookTotal { get; set; }
        public int AuthorTotal { get; set; }
        public int RequestsOutstanding { get; set; }
        public int RequestsOverdue { get; set; }
    }
}