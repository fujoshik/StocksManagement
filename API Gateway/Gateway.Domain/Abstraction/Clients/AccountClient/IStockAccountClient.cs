using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IStockAccountClient
    {
        Task BuyStockAsync(BuyStockDTO buyStock);
        Task SellStockAsync(BuyStockDTO sellStock);
        Task<decimal> CalculateAverageIncomeAsync(string stockTicker);
    }
}
