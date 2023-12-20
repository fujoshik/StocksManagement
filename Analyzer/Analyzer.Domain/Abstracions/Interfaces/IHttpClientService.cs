using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletDto> GetAccountInfoById(Guid walletId);
        Task<Stock> GetStockData(string stockTicker, string data);
        Task<List<Stock>> GetStock(string stockTicker, string startDate, string endDate);
        Task<SettlementDto> GetExecuteDeal(TransactionResponseDto transaction);
        Task<List<TransactionResponseDto>> GetTransactions(Guid accountId, string stockTicker);
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string ticker, DateTime dateTime);
    }
}