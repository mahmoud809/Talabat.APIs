using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                401 => Unauthorized(new ApiResponse(401)),
                404 => NotFound(new ApiResponse(404)),
                _ => StatusCode(code, new ApiResponse(code)) // Return ApiResponse for other status codes
            };
        }

    }
}
