
using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IService
    {
        public interface IService
        {
            Task<WalletDto> GetAccountInfoById(Guid id);
            Task<Stock> GetStockDataInternal(string stockTicker, string Data);
            public Task<List<TransactionResponseDto>> GetTransactionList(Guid accountId, string stockTicker);
            public Task<Analyzer.Domain.DTOs.TransactionResponseDto> GetTransactions(Guid accountId, string stockTicker, DateTime dateTime);
            public Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string ticker, DateTime dateTime);

            Task<SettlementDto> GetExecuteDeal(TransactionResponseDto transaction);
            //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);

        }
    }
}