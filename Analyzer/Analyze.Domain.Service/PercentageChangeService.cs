using Analyzer.Domain.Abstracions.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class PercentageChangeService : IPercentageChange
    {
        private readonly IHttpClientService httpClientService;

        public PercentageChangeService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<decimal> PercentageChange(string stockTicker, string data)
        {
            try
            {
                var stockData = await httpClientService.GetStockData(stockTicker, data);

                if (stockData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal? percentageChange = ((decimal)(stockData.LowestPrice - stockData.HighestPrice) / stockData.HighestPrice) * 100;

                return percentageChange ?? 0;  
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

    }
}
