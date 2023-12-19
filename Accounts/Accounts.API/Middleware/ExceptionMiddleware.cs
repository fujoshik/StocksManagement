using Accounts.API.Middleware.Models;
using Accounts.Domain.Exceptions;
using System.Net;

namespace Accounts.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        detail = ex.Message;
                        break;
                    case ValidationException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    case ArgumentNullException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;                  
                    case IncorrectAccountIdException:
                        statusCode = HttpStatusCode.BadRequest;
                        detail = ex.Message;
                        break;
                    case NoExistingValidatorForGivenTypeException:
                        statusCode = HttpStatusCode.NotFound;
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
