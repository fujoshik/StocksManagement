using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.User;
using Microsoft.Extensions.Logging;
using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Clients;
using Gateway.Domain.Pagination;

namespace Gateway.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IBlacklistService _blaclistservice;
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;
        private readonly ILoggingService _loggingService;

        public UserService(ILogger<UserService> logger, IBlacklistService _blaclistservice, IHttpClientFactoryCustom httpClientFactoryCustom, ILoggingService loggingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._blaclistservice = _blaclistservice;
            _httpClientFactoryCustom = httpClientFactoryCustom;
            _loggingService = loggingService;
        }

        public async Task UpdateAsync(Guid id, UserWithoutAccountIdDto user)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .GetUserAccountClient()
                .UpdateUserAsync(id, user);
        }

        public async Task<UserResponseDTO> GetByIdAsync(Guid id)
        {
            return await _httpClientFactoryCustom
                .GetAccountClient()
                .GetUserAccountClient()
                .GetUserAsync(id);
        }

        public async Task<PaginatedResult<UserResponseDTO>> GetPageAsync(Paging paging)
        {
            return await _httpClientFactoryCustom.GetAccountClient()
                .GetUserAccountClient()
                .GetPageAsync(paging);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .GetUserAccountClient()
                .DeleteAsync(id);
        }

        public bool IsEmailBlacklisted(string email)
        {
            
            bool isBlacklisted = _blaclistservice.IsEmailBlacklisted(email);

            if (isBlacklisted)
            {
                _logger.LogInformation($"Email {email} is blacklisted. Registration not allowed.");
            }

            return isBlacklisted;
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

        public UserType GetUserType(string userId)
        {
            UserStatusDTO userStatus = GetUserStatus(userId);
            decimal loadedAmount = GetLoadedAmount(userId);
            if (loadedAmount >= 100000)
            {
                return UserType.VipTrader;
            }
            else if (loadedAmount >= 50000)
            {
                return UserType.SpecialTrader;
            }
            else
            {
                return UserType.RegularTrader;
            }
        }


        public bool IsAuthenticated(string userId)
        {
            if (UserData.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool AuthenticateUser(string email, string password)
        {
            if (IsEmailBlacklisted(email))
            {
                _loggingService.LogActivity("AuthenticationAttempt", $"Authentication failed for email {email} (blacklisted).");
                return false;
            }
            bool isValidCredentials = ValidateCredentials(email, password);

            if (isValidCredentials)
            {
                _loggingService.LogActivity("AuthenticationSuccess", $"User authenticated successfully: {email}.");
                return true;
            }
            else
            {
                _loggingService.LogActivity("AuthenticationFailure", $"Authentication failed for email {email}.");
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
            bool isRegistered = RegisterUser(email, password);

            if (isRegistered)
            {
                //_trialPeriodService.StartTrialPeriod(email, 30); 

                _loggingService.LogActivity("RegistrationSuccess", $"User registered successfully: {email}.");
            }
            else
            {
                _loggingService.LogActivity("RegistrationFailure", $"Registration failed for email {email}.");
            }

            return isRegistered;
        }


        //public void Logout(string userId)
        //{
        //    if (UserExists(userId))
        //    {
        //        _loggingService.LogActivity("Logout", $"User logged out: {userId}");
        //    }
        //    else
        //    {
        //        _loggingService.LogActivity("LogoutFailure", $"Logout failed. User not found: {userId}");
        //    }
        //}

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {
            string newStatus = CalculateNewStatus(accountBalance, tradeResult);
            //_userRepository.UpdateStatus(userId, newStatus);
            _loggingService.LogActivity("UpdateUserStatus", $"User status updated: {userId}, New Status: {newStatus}");
        }


        public void UpdateUserStatus(string userId, string newStatus, decimal tradeValue)
        {
            //_userRepository.UpdateStatus(userId, newStatus);
            _loggingService.LogActivity("UpdateUserStatus", $"User status updated: {userId}, New Status: {newStatus}");
        }


    //public void UpdateUserBalance(string userId, decimal tradeValue)
    //    {
    //        if (UserExists(userId))
    //        {
    //            //UpdateBalance(userId, tradeValue);
    //            _loggingService.LogActivity("UpdateUserBalance", $"User balance updated: {userId}, Trade Value: {tradeValue}");
    //        }
    //        else
    //        {
    //            _loggingService.LogActivity("UpdateUserBalanceFailure", $"Update balance failed. User not found: {userId}");
    //        }
    //    }

        //public bool UserExists(string userId)
        //{
        //    return _userRepository.Exists(userId);
        //}

        //public void CreateUser(string userId, decimal initialBalance)
        //{
        //    if (_userService.UserExists(userId))
        //    {
        //        throw new InvalidOperationException($"User with ID {userId} already exists.");
        //    }
        //    _userService.CreateUser(userId, initialBalance);
        //    if (initialBalance == 10000)
        //    {
        //        _userService.CreateDemoAccount(userId);
        //    }

        //    ArchiveLog("UserCreation", $"User created: {userId}, Initial Balance: {initialBalance}");
        //}


        //public void CreateDemoAccount(string userId, decimal initialBalance)
        //{
        //    if (_userService.UserExists(userId))
        //    {
        //        throw new InvalidOperationException($"User with ID {userId} already exists.");
        //    }
        //    _userService.CreateUser(userId, initialBalance);
        //    _userService.CreateDemoProfile(userId);
        //    ArchiveLog("DemoAccountCreation", $"Demo account created: {userId}, Initial Balance: {initialBalance}");
        //}


        //public void DeleteAccount(string userId)
        //{
        //    if (!_userService.UserExists(userId))
        //    {
        //        throw new InvalidOperationException($"User with ID {userId} does not exist.");
        //    }
        //    var userInfo = _userService.GetUserInfo(userId);
        //    ArchiveLog("AccountDeletion", $"Account deleted: {userInfo}");
        //    _userService.DeleteUser(userId);
        //}


        public bool ValidateCredentials(string email, string password)
        {
            if (IsEmailBlacklisted(email))
            {
                throw new InvalidOperationException($"Email {email} is blacklisted and cannot be used for registration.");
            }
            bool isValidCredentials = ValidateUserCredentials(email, password);
            ArchiveLog("LoginAttempt", $"Login attempt for email: {email}, Success: {isValidCredentials}");

            return isValidCredentials;
        }

        public decimal GetLoadedAmount(string userId)
        {
            decimal loadedAmount = GetLoadedAmount(userId);
            ArchiveLog("GetLoadedAmount", $"User {userId} loaded amount: {loadedAmount}");
            return loadedAmount;
        }

        private void ArchiveLog(string activity, string details)
        {
            string logDirectory = "";
            var logFilePath = Path.Combine(logDirectory, $"Log_{DateTime.Now.ToString("yyyyMMdd")}.txt");
            File.AppendAllText(logFilePath, $"{DateTime.Now} - {activity}: {details}\n");
        }
        public void UpdateUserType(string userId, object regular)
        {
            UpdateUserType(userId, regular);
            ArchiveLog("UpdateUserType", $"User {userId} updated type to {regular}");
        }

        public void CreateDemoAccount(string userId)
        {
            DemoAccount demoAccount = new DemoAccount
            {
                UserId = userId,
                Balance = 10000, 
                CreatedAt = DateTime.UtcNow 
            };

            //_demoAccountRepository.Add(demoAccount);
            ArchiveLog("CreateDemoAccount", $"Demo account created for user {userId}");
        }


        public void CreateDemoProfile(string userId)
        {
            UserProfileDto demoProfile = new UserProfileDto
            {
                UserId = userId,
                UserType = UserType.Demo, 
                LoadedAmount = 10000,
                CreatedAt = DateTime.UtcNow 
            };
            //_userProfileRepository.Add(demoProfile);
            ArchiveLog("CreateDemoProfile", $"Demo profile created for user {userId}");
        }


        public void DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUserCredentials(string email, string password)
        {
            throw new NotImplementedException();
        }

        public object GetUserInfo(string userId)
        {
            throw new NotImplementedException();
        }

        public UserStatusDTO GetUserStatus(string userId)
        {
            throw new NotImplementedException();
        }
    }

}
