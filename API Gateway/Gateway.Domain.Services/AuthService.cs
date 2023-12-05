using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.User;

namespace Gateway.Domain.Services
{
    

    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IBlacklistService _blacklistService;
        private readonly ILoggingService _loggingService;
        private string logDirectory = "";

        public AuthService(IUserService userService, IBlacklistService blacklistService, ILoggingService loggingService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _blacklistService = blacklistService ?? throw new ArgumentNullException(nameof(blacklistService));
            _loggingService = loggingService;
        }

        public bool AuthenticateUser(string email, string password)
        {
            bool isValidUser = _userService.AuthenticateUser(email, password);

            if (isValidUser)
            {
                if (!_blacklistService.IsEmailBlacklisted(email))
                {
                    _loggingService.LogActivity("Authentication", $"User {email} authenticated successfully.");
                    return true;
                }
                else
                {
                    _loggingService.LogActivity("Authentication", $"User {email} is blacklisted.");
                    return false;
                }
            }
            else
            {
                _loggingService.LogActivity("Authentication", $"Invalid credentials for user {email}.");
                return false;
            }
        }

        public bool IsAuthenticated(string userId)
        {
            bool isAuthenticated = _userService.IsAuthenticated(userId);

            if (isAuthenticated)
            {
                _loggingService.LogActivity("Authentication", $"User {userId} is authenticated.");
                return true;
            }
            else
            {
                return false;
            }
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

        public bool RegisterUser(string email, string password)
        {
            if (IsEmailBlacklisted(email))
            {
                _loggingService.LogActivity("RegistrationAttempt", $"Registration failed for email {email} (blacklisted).");
                return false;
            }
            bool isRegistered = _userService.RegisterUser(email, password);

            if (isRegistered)
            {
                _loggingService.LogActivity("RegistrationSuccess", $"User registered successfully: {email}.");
            }
            else
            {
                _loggingService.LogActivity("RegistrationFailure", $"Registration failed for email {email}.");
            }

            return isRegistered;
        }


        public void Logout(string userId)
        {
            _userService.Logout(userId);
            _loggingService.LogActivity("Logout", $"User logged out: {userId}");
            ArchiveLog("Logout", $"User logged out: {userId}");
        }
        private void ArchiveLog(string activity, string details)
        {
            var logFilePath = Path.Combine(logDirectory, $"Log_{DateTime.Now.ToString("yyyyMMdd")}.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now} - {activity}: {details}\n");
        }

        public void DeleteAccount(string userId)
        {
            _userService.DeleteAccount(userId);
            _loggingService.LogActivity("DeleteAccount", $"User account deleted: {userId}");
            ArchiveLog("DeleteAccount", $"User account deleted: {userId}");
        }

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {
            _userService.UpdateUserStatus(userId, accountBalance, tradeResult);
            _loggingService.LogActivity("UpdateUserStatus", $"User status updated: {userId}, Balance: {accountBalance}, Trade Result: {tradeResult}");
            ArchiveLog("UpdateUserStatus", $"User status updated: {userId}, Balance: {accountBalance}, Trade Result: {tradeResult}");
        }

        public UserType GetUserType(string userId)
        {
            UserType userType = _userService.GetUserType(userId);
            if (userType == UserType.Demo && DateTime.Now - _userService.GetRegistrationDate(userId) > TimeSpan.FromDays(30))
            {
                _userService.UpdateUserType(userId, UserType.Regular);
                _loggingService.LogActivity("UserTypeUpdate", $"User type updated: {userId}, New type: Regular");
            }

            return userType;
        }

    }
}