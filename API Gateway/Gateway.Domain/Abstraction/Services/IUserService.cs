
using Gateway.Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IUserService
    {
        bool IsEmailBlacklisted(string email);
        //void CreateDemoAccount(string userId, decimal initialBalance);
        //void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        UserType GetUserType(string userId);
        bool IsAuthenticated(string userId);
        bool AuthenticateUser(string email, string password);
        bool RegisterUser(string email, string password);
        //void Logout(string userId);
        void UpdateUserStatus(string userId, string newStatus, decimal tradeValue);
        //void UpdateUserBalance(string userId, decimal tradeValue);
        //bool UserExists(string userId);
        Task UpdateAsync(Guid id, UserWithoutAccountIdDto user);
        //void CreateUser(string userId, decimal initialBalance);
        bool ValidateCredentials(string email, string password);
        decimal GetLoadedAmount(string userId);
        void UpdateUserType(string userId, object regular);
       // void CreateDemoAccount(string userId);
        //void CreateDemoProfile(string userId);
        //void DeleteUser(string userId);
        bool ValidateUserCredentials(string email, string password);
        object GetUserInfo(string userId);
        UserStatusDTO GetUserStatus(string userId);
    }
}
