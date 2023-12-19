using Gateway.Domain.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IAuthorizationService
    {
        public bool IsUserAuthorized(string userId, string route);
        public void LogRequest(string userId, string route);
        public bool IsEmailBlacklisted(string email);
        public void DeleteAccount(string userId);
        //public UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult);
        //public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
    }

}   
