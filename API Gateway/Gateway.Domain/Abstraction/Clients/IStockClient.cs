using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Clients
{
    public interface IStockClient
    {
        Task<List<StockDTO>> GetGroupedDaily();
        Task<List<StockDTO>> GetStocksByDate(string date);
        Task<StockDTO> GetStockByDateAndTicker(string date, string ticker);
        Task<StockDTO> GetStockByDateAndTickerFromAPI(string date, string ticker);
        Task<StockMarketCharacteristicsDTO> GetStockMarketCharacteristics(string date);
    }
}
