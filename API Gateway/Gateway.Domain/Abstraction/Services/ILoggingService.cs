using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface ILoggingService
    {
        void LogRequest(string userId, string route);
        void LogActivity(string userId, string activity);
    }

}