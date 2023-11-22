using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Authentication;

namespace Accounts.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IAccountService accountService,
                                     IUserService userService,
                                     IPasswordService passwordService,
                                     IUnitOfWork unitOfWork,
                                     ITokenService tokenService) 
        {
            _accountService = accountService;
            _userService = userService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async void Register(RegisterDto registerDto)
        {
            var account = await _accountService.CreateAsync(registerDto);

            await _userService.CreateAsync(registerDto, account.Id);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var accounts = await _unitOfWork.AccountRepository.GetAccountsByEmail(loginDto.Email);

            if (accounts == null || accounts.Count == 0)
            {
                return null;
            }

            var user = accounts.FirstOrDefault();

            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PasswordSalt)))
            {
                return null;
            }

            var token = _tokenService.GenerateJwtToken(user);

            return token;
        }
        
        public bool ValidateToken(string token)
        {
            return _tokenService.ValidateToken(token);
        }
    }
}
