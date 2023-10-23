using Accounts.Domain.DTOs.Account;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAuthenticationService
    {
        void Register(RegisterDto accountDto);
        Task<string> LoginAsync(LoginDto accountDto);
    }
}
