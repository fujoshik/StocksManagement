using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class DailyYieldChangesService : IDailyYieldChanges
    {
        private readonly IHttpClientService httpClientService;

        public DailyYieldChangesService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks)
        {
            try
            {
                if (stocks == null || !stocks.Any())
                {
                    throw new ArgumentException("Stocks list is null or empty.");
                }

                decimal totalDailyChanges = 0;

                foreach (var stock in stocks)
                {
                    var stockData = await httpClientService.GetStockData(stock.Ticker, stock.Date);

                    if (stockData == null)
                    {
                        throw new UserDataNotFoundException();
                    }

                    decimal? dailyChange = ((decimal)(stockData.LowestPrice - stockData.HighestPrice) / stockData.HighestPrice) * 100 ;

                    totalDailyChanges += dailyChange.Value;
                }

                decimal averageDailyChange = totalDailyChanges / stocks.Count;

                return averageDailyChange;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error calculating average daily yield changes.", ex);
            }
        }
    }
}
