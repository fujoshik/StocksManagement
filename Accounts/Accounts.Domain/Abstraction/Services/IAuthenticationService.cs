using Accounts.Domain.DTOs.Account;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAuthenticationService
    {
        void Register(RegisterDto accountDto);
        //string Login(LoginDto accountDto);
    }
}
