using Accounts.Domain.DTOs.User;

namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IUserRepository : IBaseRepository
    {
        Task DeleteByAccountIdAsync(Guid accountId);
    }
}
