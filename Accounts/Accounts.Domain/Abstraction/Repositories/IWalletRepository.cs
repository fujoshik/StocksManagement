using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IWalletRepository : IBaseRepository
    {
        Task DepositSumAsync(Guid id, DepositDto deposit);
        Task<CurrencyCode> GetCurrencyCodeAsync(Guid accountId);
        Task<decimal> GetBalanceAsync(Guid accountId, string balanceName);
        Task ChangeCurrencyCodeAsync(Guid accountId, int newCurrency,
            decimal newInitialBalance, decimal newCurrentBalance);
    }
}
