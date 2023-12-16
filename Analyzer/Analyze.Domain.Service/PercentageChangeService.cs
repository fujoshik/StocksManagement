using Accounts.Infrastructure.Entities;
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

        public async Task<decimal> PercentageChange(Guid userId, string stockTicker, string data)
        {
            try
            {
                var stockData = await httpClientService.GetStockData(stockTicker, data);
                var getTransactions = await httpClientService.GetTransactions(userId, stockTicker);

                if (stockData == null)
                {
                    throw new UserDataNotFoundException();
                }

                decimal? percentageChange = ((decimal)(stockData.OpenPrice * 100));

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