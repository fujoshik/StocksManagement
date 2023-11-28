namespace Accounts.API.Middleware
{
    public class LogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogMiddleware> _logger;

        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            using (var reader = new StreamReader(httpContext.Request.Body))
            {
                var requestBody = await reader.ReadToEndAsync();
                string logMessage = $" Path: {httpContext.Request.Path}, Method: {httpContext.Request.Method}, Body: {requestBody}";
                _logger.LogInformation(logMessage);
            }

            await _next(httpContext);
        }
    }
}
