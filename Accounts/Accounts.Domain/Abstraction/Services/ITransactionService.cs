using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Abstraction.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionResponseDto>> GetSoldTransactionsByAccountAsync(Guid accountId);
        Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId,
            string ticker, DateTime dateTime);
    }
}
