using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.HistoricalData;
using Gateway.Domain.DTOs.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IBlacklistService _blacklistService;
        private readonly ILoggingService _loggingService;
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;

        public StockService(IBlacklistService blacklistService, ILoggingService loggingService, IHttpClientFactoryCustom httpClientFactory)
        {
            _blacklistService = blacklistService;
            _loggingService = loggingService;
            _httpClientFactoryCustom = httpClientFactory;
        }

        public decimal GetCurrentStockValue()
        {

            decimal stockValue = GetStockValueFromExternalAPI();

            _loggingService.LogActivity("StockService", $"User requested current stock value: {stockValue}");

            return stockValue;
        }

        private decimal GetStockValueFromExternalAPI()
        {
            //API

            Random random = new Random();
            return (decimal)random.NextDouble() * 1000;
        }


        public IEnumerable<HistoricalDataDto> GetHistoricalData()
        {

            IEnumerable<HistoricalDataDto> historicalData = GetHistoricalDataFromSource();

            _loggingService.LogActivity("StockService", "User requested historical stock data");

            return historicalData;
        }

        private IEnumerable<HistoricalDataDto> GetHistoricalDataFromSource()
        {
            // API 

            List<HistoricalDataDto> historicalData = new List<HistoricalDataDto>
    {
        new HistoricalDataDto { Date = DateTime.Now.AddDays(-5), Value = 150 },
        new HistoricalDataDto { Date = DateTime.Now.AddDays(-4), Value = 160 },
        new HistoricalDataDto { Date = DateTime.Now.AddDays(-3), Value = 145 },
        new HistoricalDataDto { Date = DateTime.Now.AddDays(-2), Value = 170 },
        new HistoricalDataDto { Date = DateTime.Now.AddDays(-1), Value = 155 }
    };

            return historicalData;
        }

        public object GetCurrentPrice(string symbol)
        {
            throw new NotImplementedException();
        }

        public object AnalyzeStockData(string symbol)
        {
            throw new NotImplementedException();
        }

        public async Task BuyStockAsync(BuyStockDTO buyStock)
        {
            await _httpClientFactoryCustom
                .GetAccountClient()
                .BuyStockAsync(buyStock);
        }
    }
}

