using Gateway.Domain.DTOs.User;
using Gateway.Domain.Pagination;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IUserAccountClient
    {
        Task UpdateUserAsync(Guid id, UserWithoutAccountIdDto user);
        Task<UserResponseDTO> GetUserAsync(Guid id);
        Task<PaginatedResult<UserResponseDTO>> GetPageAsync(Paging pagingInfo);
        Task DeleteAsync(Guid id);
    }
}
