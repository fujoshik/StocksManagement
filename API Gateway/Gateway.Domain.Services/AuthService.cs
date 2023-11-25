using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;

namespace Gateway.Domain.Services
{
    public interface IAuthService
    {
        bool AuthenticateUser(string email, string password);
        bool IsAuthenticated(string userId);
        bool IsEmailBlacklisted(string email);
        bool RegisterUser(string email, string password);
        void Logout(string userId);
        void StartTrialPeriod(string userId);
        void DeactivateAccount(string userId);
        void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        UserType GetUserType(string userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IBlacklistService _blacklistService;
        private readonly IAccountService _accountService;

        public AuthService(IUserService userService, IBlacklistService blacklistService, IAccountService accountService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _blacklistService = blacklistService ?? throw new ArgumentNullException(nameof(blacklistService));
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        public bool AuthenticateUser(string email, string password)
        {
            return _userService.AuthenticateUser(email, password);
        }

        public bool IsAuthenticated(string userId)
        {
            return _userService.IsAuthenticated(userId);
        }

        public bool IsEmailBlacklisted(string email)
        {
            return _blacklistService.IsEmailBlacklisted(email);
        }

        public bool RegisterUser(string email, string password)
        {
            return _userService.RegisterUser(email, password);
        }

        public void Logout(string userId)
        {
            _userService.Logout(userId);
        }

        public void StartTrialPeriod(string userId)
        {
            _accountService.StartTrialPeriod(userId);
        }

        public void DeactivateAccount(string userId)
        {
            _accountService.DeactivateAccount(userId);
        }

        public void DeleteAccount(string userId)
        {
            _userService.DeleteAccount(userId);
        }

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {
            _userService.UpdateUserStatus(userId, accountBalance, tradeResult);
        }

        public UserType GetUserType(string userId)
        {
            return _userService.GetUserType(userId);
        }
    }
}