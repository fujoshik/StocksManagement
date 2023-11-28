using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Abstraction.Repositories
{
    public interface ITransactionRepository : IBaseRepository
    {
        Task DeleteByAccountIdAsync(Guid accountId);
        Task<List<TransactionResponseDto>> GetSoldTransactionsByAccountId(Guid accountId);
    }
}
