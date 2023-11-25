using Microsoft.Azure.Management.Graph.RBAC.Fluent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public interface IUserService
    {
        bool IsEmailBlacklisted(string email);
        void CreateDemoAccount(string userId, decimal initialBalance);
        void DeleteAccount(string userId);
        void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult);
        UserType GetUserType(string userId);
        bool IsAuthenticated(string userId);
        bool AuthenticateUser(string email, string password);
        bool RegisterUser(string email, string password);
        void Logout(string userId);
        void UpdateUserStatus(string userId, string newStatus, decimal tradeValue);
        void UpdateUserBalance(string userId, decimal tradeValue);
        bool UserExists(string userId);
        Task UpdateAsync(Guid id, UserWithoutAccountIdDto user);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool IsEmailBlacklisted(string email)
        {
            
            bool isBlacklisted = _blacklistService.IsEmailBlacklisted(email);

            if (isBlacklisted)
            {
                _logger.LogInformation($"Email {email} is blacklisted. Registration not allowed.");
            }

            return isBlacklisted;
        }

        public void CreateDemoAccount(string userId, decimal initialBalance)
        {
            try
            {
                
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (existingUser != null)
                {
                    _logger.LogInformation($"Demo account already exists for user {userId}");
                    return;
                }

                
                var demoAccount = new User
                {
                    UserId = userId,
                    AccountBalance = initialBalance,
                    
                };

                
                _dbContext.Users.Add(demoAccount);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Created demo account for user {userId} with initial balance {initialBalance}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating demo account for user {userId}: {ex.Message}");
                
            }
        }


        public void DeleteAccount(string userId)
        {
            try
            {
                
                var userToDelete = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (userToDelete == null)
                {
                    _logger.LogInformation($"User {userId} not found. Account deletion skipped.");
                    return;
                }

                
                _dbContext.Users.Remove(userToDelete);
                _dbContext.SaveChanges();

                _logger.LogInformation($"Deleted account for user {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting account for user {userId}: {ex.Message}");
                
            }
        }


        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {
            try
            {
                
                var userToUpdate = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

                if (userToUpdate == null)
                {
                    _logger.LogInformation($"User {userId} not found. Status update skipped.");
                    return;
                }

                
                var newStatus = CalculateNewStatus(accountBalance, tradeResult);

                
                userToUpdate.Status = newStatus;
                userToUpdate.Balance = accountBalance; 

                _dbContext.SaveChanges();

                _logger.LogInformation($"Updated status for user {userId} with balance {accountBalance} and trade result {tradeResult}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating status for user {userId}: {ex.Message}");
                
            }
        }

        private string CalculateNewStatus(decimal accountBalance, decimal tradeResult)
        {
            
            decimal vipThreshold = 100000; 
           
            if (tradeResult > vipThreshold)
            {
                return "VIP";
            }
            else
            {
               
                return "Regular";
            }
        }


    }

}
