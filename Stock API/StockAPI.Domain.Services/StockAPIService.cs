using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services
{
    public class StockAPIService:IStockAPIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<StockAPIService> _logger;

        public StockAPIService(ILogger<StockAPIService> logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.polygon.io/");
            _httpClient.DefaultRequestHeaders.Add("apiKey", "F4jmOb0acMPeUWRFcBipoI9mZ5MYhXqD");
            _apiKey = "F4jmOb0acMPeUWRFcBipoI9mZ5MYhXqD";
            _logger = logger;
        }

        public async Task<List<Stock>> GetGroupedDailyData()
        {
            try
            {
                DateTime currentDate = DateTime.Now.AddDays(-5);
                string formattedDate = currentDate.ToString("yyyy-MM-dd");
                string endpointUrl = $"/v2/aggs/grouped/locale/us/market/stocks/{formattedDate}?adjusted=true&apiKey={_apiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject<dynamic>(responseData);

                Root result = JsonConvert.DeserializeObject<Root>(json.ToString());

                var stocks = new List<Stock>();
                int i = 10;

                foreach (var item in result.results)
                {
                    if (i == 0) break;
                    stocks.Add(new Stock
                    {
                        StockTicker = item.T,
                        ClosestPrice = item.c,
                        HighestPrice = item.h,
                        LowestPrice = item.l
                    });
                    i--;
                }

                _logger.LogInformation("Successfully fetched test data.");

                return stocks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching grouped daily data.");

                throw ex;
            }
        }

        public async Task<string> Test()
        {
            try
            {
                DateTime currentDate = DateTime.Now.AddDays(-5);
                string formattedDate = currentDate.ToString("yyyy-MM-dd");
                string endpointUrl = $"v2/aggs/grouped/locale/us/market/stocks/{formattedDate}?adjusted=true&apiKey={_apiKey}";

                HttpResponseMessage response = await _httpClient.GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                _logger.LogInformation("Successfully fetched grouped daily data.");

                return responseData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching grouped daily data.");

                throw ex;
            }
        }
    }
}
