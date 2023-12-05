using API.Gateway.Controllers;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, UserData> _userDataCache = new Dictionary<string, UserData>();
        private readonly Dictionary<string, DateTime> _demoPeriodStartCache = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, DateTime> _accountExpirationCache = new Dictionary<string, DateTime>();
        private readonly Dictionary<string, List<string>> _userRequestsCache = new Dictionary<string, List<string>>();

        public string logDirectory { get; private set; }

        public void CacheUserData(string userId, UserData userData)
        {
            _userDataCache[userId] = userData;
        }

        public UserData GetCachedUserData(string userId)
        {
            return _userDataCache.TryGetValue(userId, out var userData) ? userData : null;
        }

        public void ArchiveLog(string fileName, string logEntry)
        {
            string logFilePath = Path.Combine(logDirectory, fileName);

            try
            {

                if (!File.Exists(logFilePath))
                {
                    using (var fs = File.Create(logFilePath))
                    {

                    }
                }


                File.AppendAllText(logFilePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {logEntry}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error backing up log {ex.Message}");

            }
        }

        public void TrackUserRequests(string userId, string route)
        {
            if (!_userRequestsCache.ContainsKey(userId))
            {
                _userRequestsCache[userId] = new List<string>();
            }

            _userRequestsCache[userId].Add(route);
        }

        public void ClearUserCache(string userId)
        {
            _userDataCache.Remove(userId);
            _demoPeriodStartCache.Remove(userId);
            _accountExpirationCache.Remove(userId);
            _userRequestsCache.Remove(userId);
        }

        public void CacheDemoPeriodStart(string userId, DateTime startDate)
        {
            _demoPeriodStartCache[userId] = startDate;
        }

        public DateTime GetDemoPeriodStart(string userId)
        {
            return _demoPeriodStartCache.TryGetValue(userId, out var startDate) ? startDate : DateTime.MinValue;
        }

        public void SetAccountExpiration(string userId, DateTime expirationDate)
        {
            _accountExpirationCache[userId] = expirationDate;
        }

        public DateTime GetAccountExpiration(string userId)
        {
            return _accountExpirationCache.TryGetValue(userId, out var expirationDate) ? expirationDate : DateTime.MinValue;
        }
    }

}
