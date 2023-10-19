using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.User;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateAsync(RegisterDto registerDto, Guid accountId);
    }
}
