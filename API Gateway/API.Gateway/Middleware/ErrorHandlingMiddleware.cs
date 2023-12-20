using API.Gateway.Middleware.Models;
using System.Net;

namespace API.Gateway.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string detail = "";

            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode == 404)
                {
                    statusCode = (HttpStatusCode)httpContext.Response.StatusCode;
                    detail = "Request path was not found";
                    await HandleExceptionAsync(httpContext, statusCode, detail);
                }
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case ArgumentNullException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    case UnauthorizedAccessException:
                        statusCode = HttpStatusCode.Forbidden;
                        detail = ex.Message;
                        break;
                    case HttpRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        detail = ex.Message;
                        break;
                }

                _logger.LogError($"Something went wrong: {ex}");

                await HandleExceptionAsync(httpContext, statusCode, detail);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            await context.Response.WriteAsync(new ProblemDetails()
            {
                Type = "about:blank",
                Title = code.ToString(),
                Status = context.Response.StatusCode,
                Detail = detail,
                Instance = context.Request.Path
            }.ToString());
        }
    }
}
