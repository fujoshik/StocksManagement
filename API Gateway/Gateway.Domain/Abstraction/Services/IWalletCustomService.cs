using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IWalletCustomService
    {
        Task DepositSumAsync(DepositSumDto deposit);
        Task ChangeCurrencyAsync(Currency currency);
        Task<WalletResponse> GetWalletInfoAsync(Guid id);
    }
}
