using Gateway.Domain.DTOs.User;

namespace Gateway.Domain.Abstraction.Services
{
    public interface ICacheService
    {
        void CacheUserData(string userId, UserData userData);
        UserData GetCachedUserData(string userId);
        void ArchiveLog(string fileName, string logEntry);
        void TrackUserRequests(string userId, string route);
        void ClearUserCache(string userId);
        void CacheDemoPeriodStart(string userId, DateTime startDate);
        DateTime GetDemoPeriodStart(string userId);
        void SetAccountExpiration(string userId, DateTime expirationDate);
        DateTime GetAccountExpiration(string userId);
    }

}
