using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Abstraction.Repository
{
    public interface ISettlementRepository
    {
        Task InsertTransaction(TransactionRequestDto transaction, SettlementResponseDto settlement);
        Task UpdateWalletBalance(Guid Id, decimal newBalance);
        Task<List<WalletResponseDto>> GetAllWallets();
        Task InsertHandledWallets(Guid id);
        Task<List<Guid>> GetHandledWalletIds();
        Task CreateFailedTransactionsTable();
        Task InsertIntoFailedTransaction(TransactionRequestDto transaction);
        Task<List<TransactionRequestDto>> GetFailedTransactions();
        Task DeleteFailedTransaction(Guid walletId);
    }
}
