using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Accounts.Domain.DTOs.Wallet;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient accountClient;
        private readonly HttpClient sockAPI;

        public HttpClientService()
        {
            accountClient = new HttpClient();
            accountClient.BaseAddress = new Uri("https://localhost:7073/accounts-api/wallets/");

            sockAPI = new HttpClient();
            sockAPI.BaseAddress = new Uri("http://localhost:5064");
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

        public HttpClient GetAccountClient()
        {
            return accountClient;
        }

        public HttpClient GetStockAPI()
        {
            return sockAPI;
        }
    }
}
