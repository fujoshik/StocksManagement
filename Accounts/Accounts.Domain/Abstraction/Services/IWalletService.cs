using Accounts.Domain.DTOs.Wallet;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IWalletService
    {
        Task<WalletResponseDto> CreateAsync(WalletRequestDto wallet);
    }
}
