using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletDto> GetAccountInfoById(Guid id);
        Task<Stock> GetStockData(string stockTicker, string data);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();

        //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);
    }
}