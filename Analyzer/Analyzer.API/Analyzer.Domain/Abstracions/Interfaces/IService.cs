using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
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