using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Wallet;

namespace Accounts.Domain.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<WalletResponseDto> CreateAsync(WalletRequestDto wallet)
        {
            return await _walletRepository.InsertAsync<WalletRequestDto, WalletResponseDto>(wallet);
        }
    }
}
