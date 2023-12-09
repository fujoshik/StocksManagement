using System.Net;
using Accounts.Infrastructure.Entities;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;


namespace Analyzer.API.Analyzer.Domain.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges dailyYieldChangesService;
        private readonly IPercentageChange percentageChangeService;

        public CalculationService(
            IHttpClientService httpClientService,
            IDailyYieldChanges dailyYieldChangesService,
            IPercentageChange percentageChangeService)
        {
            this.httpClientService = httpClientService;
            this.dailyYieldChangesService = dailyYieldChangesService;
            this.percentageChangeService = percentageChangeService;
        }

        public async Task<decimal> CalculateCurrentYield(Guid userId, string stockTicker, string data)
        {
            try
            {
                var stockData = await httpClientService.GetStockData(stockTicker, data);
                var transactions = await httpClientService.GetTransactions(userId);

                if (stockData != null && transactions != null && transactions.Any())
                {
                    decimal currentYield = ((decimal)stockData.OpenPrice * transactions.Sum(t => t.Quantity)) - (transactions.Sum(t => t.Quantity * t.Price));
                    return currentYield;
                }
                else
                {
                    throw new InvalidOperationException("Unable to calculate current yield. Stock or transactions data not available.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating current yield: {ex.Message}");
            }
        }




        public async Task<decimal> PercentageChange(Guid userId, string stockTicker, string data)
        {
            return await percentageChangeService.PercentageChange(userId,stockTicker, data);
        }


        public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }

    }

}
