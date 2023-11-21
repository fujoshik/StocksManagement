using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Accounts.Domain.DTOs.Wallet;
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
            accountClient.BaseAddress = new Uri("https://localhost:7073/accounts-api/wallets/{id}");

            stockApi = new HttpClient();
            stockApi.BaseAddress = new Uri("https://localhost:7195/api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stockTicker}");
        }

        public async Task<WalletResponseDto> GetUserDataById(string endpoint, Guid userId)
        {
            // Adjust the endpoint based on your API structure
            var response = await accountClient.GetAsync($"{endpoint}/{userId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<WalletResponseDto>();
            }
            else
            {
                // Handle the error or throw an exception
                throw new HttpRequestException($"Error fetching user data. Status code: {response.StatusCode}");
            }
        }

        public async Task<Stock> GetStockData(string endpoint, string stockTicker, string Data)
        {
            // Adjust the endpoint based on your API structure
            var response = await stockApi.GetAsync($"{endpoint}/{stockTicker}/{Data}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Stock>();
            }
            else
            {
                // Handle the error or throw an exception
                throw new HttpRequestException($"Error fetching user data. Status code: {response.StatusCode}");
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
    }
}
