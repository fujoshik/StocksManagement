using Gateway.Domain.DTOs.HistoricalData;
using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStockService
    {
        Task BuyStockAsync(BuyStockDTO buyStock);
        Task<List<StockDTO>> GetGroupedDailyData();
        Task<List<StockDTO>> GetStocksByDate(string date);
        Task<StockDTO> GetStockByDateAndTicker(string date, string stockTicker);
        Task<StockDTO> GetStockByDateAndTickerFromAPI(string date, string stockTicker);
        Task<StockMarketCharacteristicsDTO> GetStockMarketCharacteristics(string date);
    }
}
