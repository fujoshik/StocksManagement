using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStatisticsService
    {
        Task<decimal> CalculateAverageIncomeAsync(string stockTicker);
        int GetRequestCount(string route);
        List<string> GetTopUsersByRequests(int count);
        void LogRequest(string route);
    }
}
