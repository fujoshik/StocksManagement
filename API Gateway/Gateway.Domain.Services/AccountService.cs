using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public interface IAccountService
    {
        void CreateAccount(string userId, decimal initialBalance);
        void StartTrialPeriod(string userId, decimal demoBalance, int trialPeriodDays);
        void DeactivateAccount(string userId);
        void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        UserType GetUserType(string userId);
        void StartTrialPeriod(string userId);
        Task LoginAsync(LoginDto account);
    }
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public void CreateAccount(string userId, decimal initialBalance)
        {

            _userService.CreateUser(userId, initialBalance);
        }

        public void StartTrialPeriod(string userId, decimal demoBalance, int trialPeriodDays)
        {

            _userService.UpdateUserStatus(userId, UserType.Demo, demoBalance);

        }

        public void DeactivateAccount(string userId)
        {

            _userService.UpdateUserStatus(userId, UserType.Inactive, 0);
        }

        public void DeleteAccount(string userId)
        {

            _userService.DeleteAccount(userId);
        }

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {

            UserType newStatus = CalculateNewStatus(accountBalance, tradeResult);
            _userService.UpdateUserStatus(userId, newStatus, accountBalance);
        }

        public UserType GetUserType(string userId)
        {

            return _userService.GetUserType(userId);
        }

        private UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult)
        {
            if (accountBalance < 0)
            {
                return UserType.InDebt;
            }
            else if (tradeResult > 10000)
            {
                return UserType.VIP;
            }
            else if (accountBalance > 5000)
            {
                return UserType.Special;
            }
            else
            {
                return UserType.Regular;
            }
        }

    }


}

}
