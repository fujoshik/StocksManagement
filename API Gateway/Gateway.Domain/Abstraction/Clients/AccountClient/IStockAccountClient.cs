using Gateway.Domain.DTOs.Analyzer;
using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IStockAccountClient
    {
        Task BuyStockAsync(BuyStockDTO buyStock);
        Task SellStockAsync(BuyStockDTO sellStock);
        Task<CalculateCurrentYieldDTO> CalculateAverageIncomeAsync(string stockTicker, string date);
        Task<PercentageChangeDTO> GetPercentageChangeAsync(string stockTicker, string date);
        Task<List<DailyYieldChangeDTO>> GetDailyYieldChangesAsync(string stockTicker, string date);
    }
}
