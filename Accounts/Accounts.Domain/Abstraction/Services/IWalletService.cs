using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IWalletService
    {
        Task<WalletResponseDto> CreateAsync(WalletRequestDto wallet);
        Task<WalletResponseDto> GetWalletInfoAsync(Guid id);
        Task DepositSumAsync(DepositDto deposit);
        Task ChangeCurrencyAsync(CurrencyCode currency);
    }
}
