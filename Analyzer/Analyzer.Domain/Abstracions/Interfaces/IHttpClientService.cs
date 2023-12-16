using Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Settlement.Domain.DTOs.Settlement;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletDto> GetAccountInfoById(Guid id);
        Task<Stock> GetStockData(string stockTicker, string data);
        Task<List<Stock>> GetStock(string stockTicker, string startDate, string endDate);
        public Task<SettlementDto> GetExecuteDeal(TransactionResponseDto transaction);
        public Task<List<TransactionResponseDto>> GetTransactions(Guid accountId, string stockTicker);

        Task<HttpResponseMessage> GetAsync(string requestUri);

        Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string ticker, DateTime dateTime);


        //public Task<List<TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();
        public HttpClient GetStock();
        HttpClient GetSettlementAPI();
        HttpClient GetTransactionsDetails();
        HttpClient GetTransactions();

        //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);
    }
}