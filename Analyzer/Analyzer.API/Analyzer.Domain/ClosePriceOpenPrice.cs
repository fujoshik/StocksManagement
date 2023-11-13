using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;


namespace Analyzer.API.Analyzer.Domain
{
    public class ClosePriceOpenPrice
    {
        private readonly IHttpClientService httpClientService;

        public ClosePriceOpenPrice(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<decimal> GetStockOpeningPrice(string stockTicker)
        {
            try
            {
                var response = await httpClientService.GetStockAPI().GetAsync($"YOUR_STOCK_API_ENDPOINT/opening-price?ticker={stockTicker}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<decimal>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching opening price for stock {stockTicker}.", ex);
            }
        }

        public async Task<decimal> GetStockClosingPrice(string stockTicker)
        {
            try
            {
                var response = await httpClientService.GetStockAPI().GetAsync($"YOUR_STOCK_API_ENDPOINT/closing-price?ticker={stockTicker}");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<decimal>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error fetching closing price for stock {stockTicker}.", ex);
            }
        }
    }
}
