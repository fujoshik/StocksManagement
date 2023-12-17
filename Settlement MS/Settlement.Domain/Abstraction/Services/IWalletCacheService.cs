using Accounts.Domain.DTOs.Wallet;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IWalletCacheService
    {
        Task<WalletResponseDto> GetWalletFromCache(Guid walletId);
        Task SetWalletInCache(Guid walletId, Guid accountId, WalletResponseDto wallet);
    }
}
