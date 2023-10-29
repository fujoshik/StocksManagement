using Microsoft.Extensions.Logging;

namespace Accounts.Domain.Abstraction.Providers
{
    public interface ILoggerProvider : IDisposable
    {
        ILogger CreateLogger(string categoryName);
    }
}
