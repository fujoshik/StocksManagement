using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetUserDataById(string endpoint, Guid userId);
        Task<Stock> GetStockData(string endpoint, string stockTicker, string Data);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();
    }
}
