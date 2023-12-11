
using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IAccountService
    {
        void CreateAccount(string userId, decimal initialBalance);
        void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        void UpdateUserStatus(string userId, UserType newUserStatus);
        //UserType GetUserType(string userId);
        Task RegisterAsync(RegisterWithSumDTO account);
        Task RegisterTrialAsync(RegisterTrialDTO account);
        Task<string> LoginAsync(LoginDto loginDto);
    }
}
