using Gateway.Domain.DTOs.HistoricalData;
using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStockService
    {
        object AnalyzeStockData(string symbol);
        object GetCurrentPrice(string symbol);
        decimal GetCurrentStockValue();
        IEnumerable<HistoricalDataDto> GetHistoricalData();
        Task BuyStockAsync(BuyStockDTO buyStock);
    }
}
