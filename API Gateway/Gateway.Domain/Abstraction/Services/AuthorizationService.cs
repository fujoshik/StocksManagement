using Gateway.Domain.Services;
using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IAuthorizationService
    {
        bool IsUserAuthorized(string userId, string route);
        void LogRequest(string userId, string route);
        void TrackUserRequests(string userId, string route);
        bool IsEmailBlacklisted(string email);
        bool IsDemoAccountExpired(string userId);
        void DeleteAccount(string userId);
        UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IBlacklistService _blacklistService;
        private readonly IAccountService _accountService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AuthorizationService> _logger;

        public object UserType { get; private set; }

        public AuthorizationService(IBlacklistService blacklistService, IAccountService accountService, ICacheService cacheService, ILogger<AuthorizationService> logger)
        {
            _blacklistService = blacklistService ?? throw new ArgumentNullException(nameof(blacklistService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsUserAuthorized(string userId, string route)
        {
            
            if (!_blacklistService.IsUserBlacklisted(userId))
            {
                if (route == "StockAPI" || route == "StockHistoricalData")
                {
                    
                    return true;
                }

                
                var userType = _accountService.GetUserType(userId);
                return IsAuthorizedForRoute(userType, route);
            }

            return false;
        }

        public void LogRequest(string userId, string route)
        {
            
            _logger.LogInformation($"User {userId} requested {route}");

            
            var logFileName = DateTime.UtcNow.ToString("yyyy-MM-dd");
            _cacheService.ArchiveLog(logFileName, $"User {userId} requested {route}");
        }

        public void TrackUserRequests(string userId, string route)
        {
            
            _cacheService.TrackUserRequests(userId, route);
        }

        public bool IsEmailBlacklisted(string email)
        {
            
            return _blacklistService.IsEmailBlacklisted(email);
        }

        public bool IsDemoAccountExpired(string userId)
        {
            
            var registrationDate = _accountService.GetRegistrationDate(userId);
            return (DateTime.UtcNow - registrationDate).TotalDays > 30;
        }

        public void DeleteAccount(string userId)
        {
            
            _accountService.DeleteAccount(userId);
            _cacheService.ClearUserCache(userId);
        }

        public UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult)
        {
            
            return UserType.Regular; 
        }

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {
            
            var newUserStatus = CalculateNewStatus(accountBalance, tradeResult);
            _accountService.UpdateUserStatus(userId, newUserStatus);
        }

        private bool IsAuthorizedForRoute(UserType userType, string route)
        {
            
            switch (userType)
            {
                case UserType.Regular:
                    
                    return route != "AdminAPI"; 

                case UserType.Special:
                    
                    return true; 

                case UserType.VIP:
                    
                    return true; 

                default:
                    return false;
            }
        }
    }

}
