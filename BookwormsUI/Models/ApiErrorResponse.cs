using System.Collections.Generic;

namespace BookwormsUI.Models
{
    public class ApiErrorResponse
    {
        public List<string> Errors { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}