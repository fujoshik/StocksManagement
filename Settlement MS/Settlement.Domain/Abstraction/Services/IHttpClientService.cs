using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetWalletBalance(Guid walletId);
        Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
    }
}
