using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class ApiService : IService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges dailyYieldChangesService;
        private readonly IPercentageChange percentageChangeService;

        public ApiService(
            IHttpClientService httpClientService,
            IDailyYieldChanges dailyYieldChangesService,
            IPercentageChange percentageChangeService)
        {
            this.httpClientService = httpClientService;
            this.dailyYieldChangesService = dailyYieldChangesService;
            this.percentageChangeService = percentageChangeService;
        }

        public async Task<WalletResponseDto> GetAccountInfoById(Guid id)
        {
            using (var httpClient = httpClientService.GetAccountClient())
            {
                string getUrl = $"/accounts-api/wallets/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    WalletResponseDto accountData = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                    return accountData;
                }
                else
                {
                    throw new HttpRequestException($"Error fetching user data. Status code: {response.StatusCode}");
                }
            }
        }

        public async Task<Stock> GetStockDataInternal(string stockTicker, string date)
        {
            using (var httpClient = httpClientService.GetStockAPI())
            {
                string getUrl = $"/api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stockTicker}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    Stock stockData = JsonConvert.DeserializeObject<Stock>(data);
                    return stockData;
                }
                else
                {
                    throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
                }
            }
        }
    }
}
