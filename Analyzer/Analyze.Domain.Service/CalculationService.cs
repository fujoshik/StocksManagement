using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Accounts.Domain.Abstraction.Services;
using StockAPI.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.Abstraction.Repositories;
using Analyze.Domain.Service;



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

        

        public async Task<decimal> CalculateCurrentYieldForUser(Guid userId, string stockTicker, string Data)
        {
            try
            {
                var userData = await httpClientService.GetAccountInfoById(userId);
                var stockData = await httpClientService.GetStockData(stockTicker,Data);

                if (userData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal? closestPrice = stockData.ClosestPrice;
                decimal userCurrentBalance = userData.CurrentBalance;

                if (!IsValidMarketPrice(userCurrentBalance))
                {
                    throw new ArgumentException("Invalid current market price for the user.");
                }

                decimal userCurrentYield = (decimal)(userCurrentBalance/ closestPrice) * 100;
                return userCurrentYield;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UserDataNotFoundException();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calculating current yield for user {userId}.", ex);
            }
        }

        public decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks)
        {
            // Implement your logic for calculating portfolio risk here
            return 10.5m;
        }

        public async Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks)
        {
            return await dailyYieldChangesService.CalculateDailyYieldChanges(stocks);
        }


        public async Task<decimal> PercentageChange(string stockTicker, string data)
        {
            return await percentageChangeService.PercentageChange(stockTicker, data);
        }

        public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }



        //Task<decimal> ICalculationService.CalculateCurrentYield(Guid userId)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<decimal> ICalculationService.CalculateCurrentYieldForUser(Guid userId)
        //{
        //    throw new NotImplementedException();
        //}

        //decimal ICalculationService.CalculatePortfolioRisk(List<CalculationDTOs> stocks)
        //{
        //    throw new NotImplementedException();
        //}


        //Task<decimal> ICalculationService.FetchPercentageChange(string stockTicker, string data)
        //{
        //    throw new NotImplementedException();
        //}

        //bool ICalculationService.IsValidMarketPrice(decimal currentBalance)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<decimal> ICalculationService.CalculateDailyYieldChanges(List<CalculationDTOs> stocks)
        //{
        //    throw new NotImplementedException();
        //}
    }
    
}
