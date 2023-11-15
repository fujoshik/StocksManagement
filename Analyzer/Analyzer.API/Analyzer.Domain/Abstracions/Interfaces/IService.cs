using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IService
    {
        Task<WalletResponseDto> GetAccountInfoById(Guid id);

        //Task<> GetInfoFromSettlement(string id);

        Task<Stock> GetStockData(string stockTicker, string Data);
    }
}
