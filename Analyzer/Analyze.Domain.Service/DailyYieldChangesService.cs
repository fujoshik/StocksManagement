using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analyzer.Domain.DTOs;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class DailyYieldChangesService : IDailyYieldChanges
    {
        private readonly IHttpClientService httpClientService;
       

        public DailyYieldChangesService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
            
        }

        public List<decimal> CalculateDailyYieldChanges(List<StockDTO> stockDataList)
        {
            List<decimal> dailyYieldChanges = new List<decimal>();

            foreach (var stockData in stockDataList)
            {
                decimal dailyYieldChange = (stockData.CurrentPrice * stockData.Quantity - stockData.OpenPrice * stockData.Quantity) /
                                           (stockData.OpenPrice * stockData.Quantity);

                dailyYieldChanges.Add(dailyYieldChange);
            }

            return dailyYieldChanges;
        }

    }
}
