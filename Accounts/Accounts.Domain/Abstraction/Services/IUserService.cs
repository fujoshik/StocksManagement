using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.User;
using Accounts.Domain.Pagination;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> CreateAsync(RegisterDto registerDto, Guid accountId);
        Task<UserResponseDto> UpdateAsync(Guid id, UserWithoutAccountIdDto user);
        Task<UserResponseDto> GetByIdAsync(Guid id);
        Task<PaginatedResult<UserResponseDto>> GetPageAsync(PagingInfo pagingInfo);
        Task DeleteAsync(Guid id);
    }
}
