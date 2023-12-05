using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Authentication;
using Accounts.Domain.DTOs.MongoDB;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IMongoDBService _mongoDBService;
        private readonly IMapper _mapper;

        public AuthenticationService(IAccountService accountService,
                                     IUserService userService,
                                     IPasswordService passwordService,
                                     IUnitOfWork unitOfWork,
                                     ITokenService tokenService,
                                     IEmailService emailService,
                                     IMongoDBService mongoDBService,
                                     IMapper mapper) 
        {
            _accountService = accountService;
            _userService = userService;
            _passwordService = passwordService;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _emailService = emailService;
            _mongoDBService = mongoDBService;
            _mapper = mapper;
        }

        public async Task SendVerificationEmailAsync(RegisterDto registerDto)
        {
            var verificationCode = Guid.NewGuid().ToString();
            _emailService.SendEmail(registerDto.Email, verificationCode);

            UserDto userDto;

            if (registerDto is RegisterWithSumDto)
            {
                userDto = _mapper.Map<UserDto>(registerDto as RegisterWithSumDto);
            }
            else
            {
                userDto = _mapper.Map<UserDto>(registerDto as RegisterTrialDto);
            }

            userDto.VerificationCode = verificationCode;
            
            await _mongoDBService.CreateUserAsync(userDto);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var accounts = await _unitOfWork.AccountRepository.GetAccountsByEmail(loginDto.Email);

            if (accounts == null || accounts.Count == 0)
            {
                return null;
            }

            var user = accounts.FirstOrDefault();

            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash, 
                Convert.FromBase64String(user.PasswordSalt)))
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

        public async Task<bool> VerifyCodeAsync(string code)
        {
            var user = await _mongoDBService.GetUserByCodeAsync(code);

            if (user == null)
            {
                return false;
            }

            if (user.Sum == 0 || user.Sum == null)
            {
                await RegisterAccountAsync(_mapper.Map<RegisterTrialDto>(user));
            }
            else
            {
                await RegisterAccountAsync(_mapper.Map<RegisterWithSumDto>(user));
            }

            return true;
        }

        private async Task RegisterAccountAsync(RegisterDto registerDto)
        {
            var account = await _accountService.CreateAsync(registerDto);

            await _userService.CreateAsync(registerDto, account.Id);
        }
    }
}
