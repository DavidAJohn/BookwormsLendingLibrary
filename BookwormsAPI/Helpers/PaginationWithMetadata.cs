using System.Collections.Generic;

namespace BookwormsAPI.Helpers
{
    public class PaginationWithMetadata<T> where T : class
    {
        public PaginationWithMetadata(int pageIndex, int pageSize, int count, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}