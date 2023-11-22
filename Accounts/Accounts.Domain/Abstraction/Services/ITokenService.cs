using Accounts.Domain.DTOs.Account;

namespace Accounts.Domain.Abstraction.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(AccountDto dto);
        bool ValidateToken(string authToken);
    }
}
