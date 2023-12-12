using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly ILoggingService _loggingService;
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;

        public StockService(ILoggingService loggingService, IHttpClientFactoryCustom httpClientFactory)
        {
            _loggingService = loggingService;
            _httpClientFactoryCustom = httpClientFactory;
        }

        public async Task BuyStockAsync(BuyStockDTO buyStock)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .BuyStockAsync(buyStock);
        }

        public async Task<List<StockDTO>> GetGroupedDailyData()
        {
            return await _httpClientFactoryCustom
                .GetStockClient()
                .GetGroupedDaily();
        }

        public async Task<StockDTO> GetStockByDateAndTickerFromAPI(string date, string stockTicker)
        {
            return await _httpClientFactoryCustom
                .GetStockClient()
                .GetStockByDateAndTickerFromAPI(date, stockTicker);
        }

        public async Task<StockDTO> GetStockByDateAndTicker(string date, string stockTicker)
        {
            return await _httpClientFactoryCustom
                .GetStockClient()
                .GetStockByDateAndTicker(date, stockTicker);
        }

        public async Task<List<StockDTO>> GetStocksByDate(string date)
        {
            return await _httpClientFactoryCustom
                .GetStockClient()
                .GetStocksByDate(date);
        }

        public async Task<StockMarketCharacteristicsDTO> GetStockMarketCharacteristics(string date)
        {
            return await _httpClientFactoryCustom
                .GetStockClient()
                .GetStockMarketCharacteristics(date);
        }
    }
}

