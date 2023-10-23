using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Accounts.Domain.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly IAccountRepository _accountRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(IAccountService accountService,
                                     IUserService userService,
                                     IPasswordService passwordService,
                                     IAccountRepository repository,
                                     JwtSettings jwtSettings) 
        {
            _accountService = accountService;
            _userService = userService;
            _passwordService = passwordService;
            _accountRepository = repository;
            _jwtSettings = jwtSettings;
        }

        public async void Register(RegisterDto registerDto)
        {
            var account = await _accountService.CreateAsync(registerDto);

            await _userService.CreateAsync(registerDto, account.Id);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var accounts = await _accountRepository.GetAccountsByEmail(loginDto.Email);

            if (accounts == null || accounts.Count == 0)
            {
                return null;
            }

            var user = accounts.FirstOrDefault();

            if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash, Convert.FromBase64String(user.PasswordSalt)))
            {
                return null;
            }

            var token = GenerateJwtToken(user);

            return token;
        }

        private IEnumerable<Claim> GetClaims(AccountDto dto)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, dto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, dto.Email),
                new Claim(JwtRegisteredClaimNames.NameId, dto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        private string GenerateJwtToken(AccountDto dto)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: GetClaims(dto),
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }
    }
}
