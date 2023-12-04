using Accounts.Domain.DTOs.Transaction;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using Newtonsoft.Json;
using StockAPI.Infrastructure.Models;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient accountClient;
        private readonly HttpClient stockApi;

        public HttpClientService()
        {
            accountClient = new HttpClient();
            accountClient.BaseAddress = new Uri("https://localhost:7073");

            stockApi = new HttpClient();
            stockApi.BaseAddress = new Uri("https://localhost:7195");
        }

        public async Task<WalletDto> GetAccountInfoById(Guid id)
        {
            using (var httpClient = GetAccountClient())
            {
                string getUrl = $"/accounts-api/wallets/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    WalletDto accountData = JsonConvert.DeserializeObject<WalletDto>(data);
                    return accountData;
                }
                else
                {
                    throw new HttpRequestException($"Error fetching user data. Status code: {response.StatusCode}");
                }
            }
        }

        public async Task<Stock> GetStockData(string stockTicker, string data)
        {
            using (var httpClient = GetStockAPI())
            {
                string getUrl = $"/api/StockAPI/get-stock-by-date-and-ticker?date={data}&stockTicker={stockTicker}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string stockData = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Stock>(stockData);
                }
                else
                {
                    throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
                }
            }
        }

        public HttpClient GetAccountClient()
        {
            return accountClient;
        }

        public HttpClient GetStockAPI()
        {
            return stockApi;
        }

    //    public Task<List<TransactionResponseDto>> GetTransactionsForUserAndStockAsync(Guid userId, string stockTicker)
    //    {
    //        throw new NotImplementedException();
    //    }
    }
}
