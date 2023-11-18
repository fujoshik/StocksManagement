using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

        public async Task<decimal> CalculateCurrentYieldForUser(Guid userId)
        {
            try
            {
                var userData = await httpClientService.GetAccountInfoById(userId);

                if (userData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal userInitialBalance = userData.InitialBalance;
                decimal userCurrentBalance = userData.CurrentBalance;

                if (!IsValidMarketPrice(userCurrentBalance))
                {
                    throw new ArgumentException("Invalid current market price for the user.");
                }

                decimal userCurrentYield = (userInitialBalance / userCurrentBalance) * 100;
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

        public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }

        public async Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks)
        {
            return await dailyYieldChangesService.CalculateDailyYieldChanges(stocks);
        }

        public async Task<decimal> FetchPercentageChange(string stockTicker, string data)
        {
            return await percentageChangeService.FetchPercentageChange(stockTicker, data);
        }

        public decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks)
        {
            // Implement your logic for calculating portfolio risk here
            return 10.5m;
        }
    }
}
