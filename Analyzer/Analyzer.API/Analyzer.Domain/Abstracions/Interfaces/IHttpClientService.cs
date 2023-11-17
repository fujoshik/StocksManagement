using System;
using System.Net.Http;
using System.Threading.Tasks;
using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetAccountInfoById(Guid id);
        Task<Stock> GetStockData(string stockTicker, string data);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();
    }
}