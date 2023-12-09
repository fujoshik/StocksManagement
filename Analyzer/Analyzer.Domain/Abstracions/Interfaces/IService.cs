
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
            public Task<List<TransactionResponseDto>> GetTransactionList(Guid userId);
            public Task<List<Analyzer.Domain.DTOs.TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker);


            //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);

        }
    }
}