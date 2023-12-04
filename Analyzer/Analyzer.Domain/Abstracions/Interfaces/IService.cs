
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.DTOs.Wallet;
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

           
            //Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker);
            
        }
    }
}