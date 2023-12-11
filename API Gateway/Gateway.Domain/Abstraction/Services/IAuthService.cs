
using Gateway.Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IAuthService
    {
        bool AuthenticateUser(string email, string password);
        bool IsAuthenticated(string userId);
        bool IsEmailBlacklisted(string email);
        bool RegisterUser(string email, string password);
        void Logout(string userId);
        void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        //UserType GetUserType(string userId);
    }
}
