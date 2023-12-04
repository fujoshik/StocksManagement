using Accounts.Domain.DTOs.Authentication;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAuthenticationService
    {
        Task SendVerificationEmailAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto accountDto);
        bool ValidateToken(string token);
        Task<bool> VerifyCodeAsync(string code);
    }
}
