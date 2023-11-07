using Accounts.Domain.DTOs.Authentication;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAuthenticationService
    {
        void Register(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto accountDto);
    }
}
