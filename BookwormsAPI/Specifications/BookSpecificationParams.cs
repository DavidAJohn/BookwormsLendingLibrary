namespace BookwormsAPI.Specifications
{
    public class BookSpecificationParams
    {
        private const int MaxPageSize = 25;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 5;

        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public string Sort { get; set; }
        private string _search;
        public string Search 
        { 
            get => _search; 
            set => _search = value.ToLower(); 
        }

        public bool IsActive { get; set; }
    }
}