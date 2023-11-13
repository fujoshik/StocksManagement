using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;

namespace Analyzer.API.Analyzer.Domain
{
    public class PercentageChangeCalculator
    {
        private readonly IHttpClientService httpClientService;

        public PercentageChangeCalculator(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<decimal> CalculatePercentageChange(Guid userId, string stockTicker)
        {
            try
            {
                decimal highestPrice = await GetStockHighestPrice(userId, stockTicker);
                decimal lowestPrice = await GetStockLowestPrice(userId, stockTicker);

                decimal percentageChange = ((lowestPrice - highestPrice) / highestPrice) * 100;

                return percentageChange;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error calculating percentage change for stock {stockTicker}.", ex);
            }
        }

        public async Task<decimal> GetStockHighestPrice(Guid userId, string stockTicker)
        {
            try
            {
                var response = await httpClientService.GetStockAPI().GetAsync($"highest-price?userId={userId}&ticker={stockTicker}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<decimal>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching highest price for stock {stockTicker}.", ex);
            }
        }

        public async Task<decimal> GetStockLowestPrice(Guid userId, string stockTicker)
        {
            try
            {
                var response = await httpClientService.GetStockAPI().GetAsync($"lowest-price?userId={userId}&ticker={stockTicker}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<decimal>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching lowest price for stock {stockTicker}.", ex);
            }
        }
    }
}
