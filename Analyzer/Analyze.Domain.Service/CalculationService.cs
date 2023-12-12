using System;
using System.Threading.Tasks;
using Accounts.Infrastructure.Entities;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using StockAPI.Domain.Abstraction.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Analyzer.API.Analyzer.Domain.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges dailyYieldChangesService;
        private readonly IPercentageChange percentageChangeService;
        private readonly IService service;

        public CalculationService(
            IHttpClientService httpClientService,
            IDailyYieldChanges dailyYieldChangesService,
            IPercentageChange percentageChangeService,
            IService service)
        {
            this.httpClientService = httpClientService;
            this.dailyYieldChangesService = dailyYieldChangesService;
            this.percentageChangeService = percentageChangeService;
            this.service = service;
        }

        public async Task<decimal> CalculateCurrentYield(Guid userId, string stockTicker, string data, TransactionResponseDto transaction)
        {
            try
            {
                var settlementDto = await httpClientService.GetTransactions(transaction);

                decimal stockPrice = settlementDto.StockPrice;
                decimal totalBalance = settlementDto.TotalBalance;

                var stockData = await httpClientService.GetStockData(stockTicker, data);

                if (stockData != null)
                {
                    decimal currentYield = ((decimal)stockData.OpenPrice * totalBalance);
                    return currentYield;
                }
                else
                {
                    throw new InvalidOperationException("Unable to calculate current yield. Stock data not available.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating current yield: {ex.Message}");
            }
        }

        



        public async Task<decimal> PercentageChange(Guid userId, string stockTicker, string data)
        {
            return await percentageChangeService.PercentageChange(userId, stockTicker, data);
        }

        public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }
    }
}
