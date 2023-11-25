using API.Gateway.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStockService
    {
        decimal GetCurrentStockValue();
        IEnumerable<HistoricalData> GetHistoricalData();
    }

    public class StockService : IStockService
    {
        private readonly IBlacklistService _blacklistService;
        private readonly ILoggingService _loggingService;

        public StockService(IBlacklistService blacklistService, ILoggingService loggingService)
        {
            _blacklistService = blacklistService;
            _loggingService = loggingService;
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


        public IEnumerable<HistoricalData> GetHistoricalData()
        {
            
            IEnumerable<HistoricalData> historicalData = GetHistoricalDataFromSource();

            _loggingService.LogActivity("StockService", "User requested historical stock data");

            return historicalData;
        }

        private IEnumerable<HistoricalData> GetHistoricalDataFromSource()
        {
            // API 

            List<HistoricalData> historicalData = new List<HistoricalData>
    {
        new HistoricalData { Date = DateTime.Now.AddDays(-5), Value = 150 },
        new HistoricalData { Date = DateTime.Now.AddDays(-4), Value = 160 },
        new HistoricalData { Date = DateTime.Now.AddDays(-3), Value = 145 },
        new HistoricalData { Date = DateTime.Now.AddDays(-2), Value = 170 },
        new HistoricalData { Date = DateTime.Now.AddDays(-1), Value = 155 }
    };

            return historicalData;
        }

    }

}
