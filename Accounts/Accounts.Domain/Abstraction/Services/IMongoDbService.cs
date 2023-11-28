using Accounts.Domain.DTOs.MongoDB;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IMongoDBService
    {
        Task<UserDto> GetUserByCodeAsync(string email);
        Task CreateUserAsync(UserDto user);
    }
}
