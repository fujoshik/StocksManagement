
using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IService
    {
        public interface IService
        {
            Task<WalletResponseDto> GetAccountInfoById(Guid id);
            Task<Stock> GetStockDataInternal(string stockTicker, string Data);
        }
    }
}