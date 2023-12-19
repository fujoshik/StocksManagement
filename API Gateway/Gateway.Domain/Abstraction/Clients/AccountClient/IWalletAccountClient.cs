using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IWalletAccountClient
    {
        Task DepositSumAsync(DepositSumDto deposit);
        Task ChangeCurrencyAsync(Currency currency);
        Task<WalletResponse> GetWalletInfoAsync(Guid id);
    }
}
