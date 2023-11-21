using StockAPI.Infrastructure.Models;

namespace Accounts.Domain.Abstraction.Clients
{
    public interface IStockApiClient
    {
        HttpClient GetStockApiClient();
        Task<Stock> GetStockByDateAndTicker(string date, string stockTicker);
    }
}
