using Accounts.Domain.DTOs.Wallet;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetWalletBalance(Guid walletId);
        //Task<....> GetStockPrice(string stockId);
    }
}
