using System.Net;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.Abstracions.Interfaces;


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
                var userData = await httpClientService.GetAccountInfoById(userId);
                var stockData = await httpClientService.GetStockData(stockTicker, data);

                if (userData == null || userData.UserData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal? closestPrice = stockData.ClosestPrice;
                decimal userCurrentBalance = userData.CurrentBalance;

                if (!IsValidMarketPrice(userCurrentBalance))
                {
                    throw new ArgumentException("Invalid current market price for the user.");
                }

                // Calculate Annual Income
                decimal annualIncome = CalculateAnnualIncome(userData.UserData.CalculationDTO?.DividendTransactions);

                if (annualIncome == 0)
                {
                    // Avoid division by zero
                    return 0;
                }

                // Calculate Current Yield
                decimal currentYield = ( annualIncome / userCurrentBalance) * 100;

                return currentYield;
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


        private decimal CalculateAnnualIncome(object dividendTransactions)
        {
            throw new NotImplementedException();
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
