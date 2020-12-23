using BookwormsAPI.Errors;
using Microsoft.AspNetCore.Mvc;

namespace BookwormsAPI.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // so OpenAPI/Swagger will ignore this controller
    public class ErrorsController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }

}