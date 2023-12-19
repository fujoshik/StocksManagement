using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.DTOs.Handled;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Abstraction.Repository
{
    public interface ISettlementRepository
    {
        Task InsertTransaction(TransactionRequestDto transaction, SettlementResponseDto settlement);
        Task UpdateWalletBalance(Guid Id, decimal newBalance);
        Task InsertHandledWallets(Guid walletId, Guid accountId, Guid transactionId);
        Task CreateFailedTransactionsTable();
        Task InsertIntoFailedTransaction(TransactionRequestDto transaction);
        Task<List<TransactionRequestDto>> GetFailedTransactions();
        Task DeleteFailedTransaction(Guid walletId);
        Task<bool> CheckValidWalletId(Guid walletId);
        Task CreateHandledWalletsTable();
        Task<List<HandledWalletsDto>> GetHandledWalletIds();
        Task<WalletResponseDto> GetWalletById(Guid walletId);
        Task<AccountResponseDto> GetAccountById(Guid accountId);
        Task UpdateTransaction(TransactionRequestDto transaction);
        Task<TransactionRequestDto> GetTransactionById(Guid transactionId);
    }
}
