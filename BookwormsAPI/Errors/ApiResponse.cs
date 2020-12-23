namespace BookwormsAPI.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode); // if null, get default
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "You have made an invalid request",
                401 => "You are not authorised to make this request",
                404 => "You have requested a resource that can not be found",
                500 => "A server error has occurred",
                _ => null
            };
        }
    }
}