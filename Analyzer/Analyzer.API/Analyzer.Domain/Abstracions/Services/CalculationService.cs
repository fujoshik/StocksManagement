using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain;
using Accounts.Domain.DTOs.Wallet;
using System.Net;
using YourNamespace;

namespace Analyzer.API.Analyzer.Domain.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IHttpClientService httpClientAccounts;
        private readonly ClosePriceOpenPrice closePriceOpenPrice;
        private readonly PercentageChangeCalculator percentageChangeCalculator;

        public CalculationService(IHttpClientService httpClientAccounts, ClosePriceOpenPrice closePriceOpenPrice)
        {
            this.httpClientAccounts = httpClientAccounts;
            this.percentageChangeCalculator = percentageChangeCalculator;
        }

        public async Task<decimal> CalculateCurrentYieldForUser(Guid userId)
        {
            try
            {
                var userData = await httpClientAccounts.GetUserDataById("/accounts-api/wallets/{id}", userId);

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
                throw;
            }
        }
        public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }

        public async Task<decimal> CalculatePercentageChange(Guid userId, string stockTicker, string Data)
        {
            try
            {
                var stockData = await httpClientAccounts.GetStockData("/api/StockAPI/get-stock-by-date-and-ticker?date={Data}&stockTicker={stockTicker}", stockTicker, Data);

                if (stockData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal highestPrice = await percentageChangeCalculator.GetStockHighestPrice(userId, stockTicker, Data);
                decimal lowestPrice = await percentageChangeCalculator.GetStockLowestPrice(userId, stockTicker, Data);

                decimal percentageChange = ((lowestPrice - highestPrice) / highestPrice) * 100;

                return percentageChange;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                throw new UserDataNotFoundException();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calculating percentage change for stock {stockTicker}.", ex);
            }
        }

        public decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks)
        {
            return 10.5m;
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
                    decimal openingPrice = await closePriceOpenPrice.GetStockOpeningPrice(stock.Ticker);
                    decimal closingPrice = await closePriceOpenPrice.GetStockClosingPrice(stock.Ticker);

                    decimal dailyChange = closingPrice - openingPrice;

                    totalDailyChanges += dailyChange;
                }

                decimal averageDailyChange = totalDailyChanges / stocks.Count;

                return averageDailyChange;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error calculating daily yield changes.", ex);
            }
        }



    }
}