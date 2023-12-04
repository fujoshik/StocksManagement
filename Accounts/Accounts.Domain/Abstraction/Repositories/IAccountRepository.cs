using Accounts.Domain.DTOs.Account;

namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IAccountRepository : IBaseRepository
    {
        Task<List<AccountDto>> GetAccountsByEmail(string email);
        Task UpdateRoleAsync(Guid id, int role);
        Task DeleteDateToDeleteAsync(Guid id);
    }
}
