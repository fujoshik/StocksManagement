using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public interface IStatisticsService
    {
        int GetRequestCount(string route);
        List<string> GetTopUsersByRequests(int count);
        
    }
    public class StatisticsService : IStatisticsService
    {
        private readonly Dictionary<string, int> _requestCounts = new Dictionary<string, int>();
        private readonly Dictionary<string, List<string>> _userRequests = new Dictionary<string, List<string>>();

        public int GetRequestCount(string route)
        {
            return _requestCounts.TryGetValue(route, out int count) ? count : 0;
        }

        public List<string> GetTopUsersByRequests(int count)
        {
            return _userRequests.OrderByDescending(kv => kv.Value.Count)
                                .Take(count)
                                .Select(kv => kv.Key)
                                .ToList();
        }

        public void LogRequest(string userId, string route)
        {
            
            if (_requestCounts.ContainsKey(route))
                _requestCounts[route]++;
            else
                _requestCounts[route] = 1;

            
            if (_userRequests.ContainsKey(userId))
                _userRequests[userId].Add(route);
            else
                _userRequests[userId] = new List<string> { route };

            //services.AddSingleton<IStatisticsService, StatisticsService>();
        }
    }

}
