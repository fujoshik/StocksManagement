using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IRequestLoggingService
    {
        void LogRequest(string userId, string route);

    }
}
