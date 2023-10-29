using Accounts.Domain.DTOs.Wallet;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetAccountBalance(Guid id);
    }
}
