using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
    //By Convension
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger , IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                //Take An Action With The Request

                await _next.Invoke(httpContext); // Go To The Next Middleware. [It's like a trap to catch exception that will be come back as a response]

                //Take An Action With The Response
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message); // That, If I Was In Development Env
                //Log Exception Will Be Stored In [Database | Files] => That, If I Was In Production Env To Make Support Team Handle it later.
                

                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var response = _env.IsDevelopment() ?
                     new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                    :
                     new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);


                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response , options);

                await httpContext.Response.WriteAsync(json);
            }
        }

    }
}
