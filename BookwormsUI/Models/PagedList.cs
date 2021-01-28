using System.Collections.Generic;

namespace BookwormsUI.Models
{
    public class PagedList<T> where T : class
    {
        public List<T> Items { get; set; }
        public PagingMetadata Metadata { get; set; }
    }
}