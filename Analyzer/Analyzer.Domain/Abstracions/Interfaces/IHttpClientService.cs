using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletDto> GetAccountInfoById(Guid id);
        Task<Stock> GetStockData(string stockTicker, string data);
        public Task<List<TransactionResponseDto>> GetTransactions(Guid walletId);
        public Task<List<TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();
        HttpClient GetSettlementAPI();
        HttpClient GetTransactionsDetails();

        //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);
    }
}