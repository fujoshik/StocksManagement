using Accounts.API.Middleware;

namespace Accounts.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        public static void ConfigureSafelistMiddleware(this WebApplication app, string safelist)
        {
            app.UseMiddleware<SafelistMiddleware>(safelist);
        }

        public static void ConfigureLogMiddleware(this WebApplication app)
        {
            app.UseMiddleware<LogMiddleware>();
        }
    }
}
