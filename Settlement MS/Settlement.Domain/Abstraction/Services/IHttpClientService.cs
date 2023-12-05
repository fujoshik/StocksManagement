using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.DTOs.Transaction;
using StockAPI.Infrastructure.Models;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetWalletBalance(Guid walletId, TransactionRequestDto transaction);
        Task<Stock> GetStockByDateAndTicker(string date, string stockTicker);
    }
}
