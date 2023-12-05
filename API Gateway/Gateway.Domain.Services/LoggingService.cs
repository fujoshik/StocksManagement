using Gateway.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ICacheService _cacheService;

        public LoggingService(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public void LogRequest(string userId, string route)
        {

            string logEntry = $"User {userId} requested route {route}";
            _cacheService.ArchiveLog("request_log.txt", logEntry);
        }

        public void LogActivity(string userId, string activity)
        {

            string logEntry = $"User {userId} performed activity: {activity}";
            _cacheService.ArchiveLog("activity_log.txt", logEntry);
        }
    }

}


