using Gateway.Domain.Abstraction.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    

    public class RequestLoggingService : IRequestLoggingService
    {
        private readonly ILogger<RequestLoggingService> _logger;

        public RequestLoggingService(ILogger<RequestLoggingService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void LogRequest(string userId, string route)
        {
            //serilog
            _logger.LogInformation($"User: {userId}, Route: {route}, Time: {DateTime.UtcNow}");
        }
    }

}
