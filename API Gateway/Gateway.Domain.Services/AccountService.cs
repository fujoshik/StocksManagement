using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.DTOs.User;


namespace Gateway.Domain.Services
{

    public class AccountService : IAccountService
    {
        //private readonly IUserService _userService;
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;

        public AccountService(//IUserService userService, 
            IHttpClientFactoryCustom httpClientFactoryCustom)
        {
            //_userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _httpClientFactoryCustom = (IHttpClientFactoryCustom?)httpClientFactoryCustom;
        }

        public void CreateAccount(string userId, decimal initialBalance)
        {

            //_userService.CreateUser(userId, initialBalance);
        }


        public void DeleteAccount(string userId)
        {

            //_userService.DeleteAccount(userId);
        }

        public void UpdateUserStatus(string userId, decimal accountBalance, decimal tradeResult)
        {

            UserType newStatus = CalculateNewStatus(accountBalance, tradeResult);
            //_userService.UpdateUserStatus(userId, newStatus, accountBalance);
        }

        //public UserType GetUserType(string userId)
        //{

        //    return _userService.GetUserType(userId);
        //}

        private UserType CalculateNewStatus(decimal accountBalance, decimal tradeResult)
        {
            if (accountBalance < 0)
            {
                return UserType.InDebt;
            }
            else if (tradeResult > 10000)
            {
                return UserType.VipTrader;
            }
            else if (accountBalance > 5000)
            {
                return UserType.SpecialTrader;
            }
            else
            {
                return UserType.RegularTrader;
            }
        }

        public void UpdateUserStatus(string userId, UserType newUserStatus)
        {
            throw new NotImplementedException();
        }
        public async Task RegisterAsync(RegisterWithSumDTO registerDto)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .GetAuthenticateAccountClient()
                .RegisterAsync(registerDto);
        }

        public async Task RegisterTrialAsync(RegisterTrialDTO registerDto)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .GetAuthenticateAccountClient()
                .RegisterTrialAsync(registerDto);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            return await _httpClientFactoryCustom
                .GetAccountClient()
                .GetAuthenticateAccountClient()
                .LoginAsync(loginDto);
        }

        public async Task VerifyCodeAsync(string code)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .GetAuthenticateAccountClient()
                .VerifyCodeAsync(code);
        }
    }
}


