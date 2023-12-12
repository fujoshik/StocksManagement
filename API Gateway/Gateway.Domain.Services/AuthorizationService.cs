using Gateway.Domain.Abstraction.Services;
using Microsoft.Extensions.Logging;
using Gateway.Domain.DTOs.User;


namespace Gateway.Domain.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IBlacklistService _blacklistService;
        private readonly IAccountService _accountService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly ILoggingService _loggingService;

        public object UserType { get; private set; }

        public AuthorizationService(IBlacklistService blacklistService, IAccountService accountService, ICacheService cacheService, ILogger<AuthorizationService> logger, ILoggingService loggingService)
        {
            _blacklistService = blacklistService ?? throw new ArgumentNullException(nameof(blacklistService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _loggingService = loggingService;
        }

        public bool IsUserAuthorized(string userId, string route)
        {

            if (!_blacklistService.IsUserBlacklisted(userId))
            {
                if (route == "StockAPI" || route == "StockHistoricalData")
                {

                    return true;
                }


                //var userType = _accountService.GetUserType(userId);
                //return IsAuthorizedForRoute(userType, route);
            }

            return false;
        }

        public void LogRequest(string userId, string route)
        {

            _logger.LogInformation($"User {userId} requested {route}");


            var logFileName = DateTime.UtcNow.ToString("yyyy-MM-dd");
            _cacheService.ArchiveLog(logFileName, $"User {userId} requested {route}");
        }

        public bool IsEmailBlacklisted(string email)
        {
            bool isBlacklisted = _blacklistService.IsEmailBlacklisted(email);

            if (isBlacklisted)
            {
                _loggingService.LogActivity("BlacklistCheck", $"Email {email} is blacklisted.");
                return true;
            }
            else
            {
                return false;
            }
        }
        public void DeleteAccount(string userId)
        {

            _accountService.DeleteAccount(userId);
            _cacheService.ClearUserCache(userId);
        }

        //public UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult)
        //{

        //    return UserType.Regular;
        //}

        //public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        //{

        //    var newUserStatus = CalculateNewStatus(accountBalance, tradeResult);
        //    _accountService.UpdateUserStatus(userId, newUserStatus);
        //}

        //private bool IsAuthorizedForRoute(UserType userType, string route)
        //{
        //    switch (userType)
        //    {
        //        case UserType.Regular:
        //            return route != "AdminAPI";
        //        case UserType.Special:
        //            return true;
        //        case UserType.Vip:
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

    }
}


