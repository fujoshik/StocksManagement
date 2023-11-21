using Settlement.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Wallet;
using Newtonsoft.Json;
using StockAPI.Infrastructure.Models;

namespace Settlement.Domain.Services
{
    public class ConnectionService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly IWalletRoutes walletRoutes;
        private readonly IStockRoutes stockRoutes;

        public ConnectionService(HttpClient httpClient, IWalletRoutes walletRoutes, IStockRoutes stockRoutes)
        {
            this.httpClient = httpClient;
            this.walletRoutes = walletRoutes;
            this.stockRoutes = stockRoutes;
        }

        public async Task<WalletResponseDto> GetWalletBalance(Guid walletId)
        {
            //httpClient.DefaultRequestHeaders.Add("X-Api-Name", "Accounts.API"); // Identifies the source of the API.

            HttpResponseMessage response = await httpClient.GetAsync(walletRoutes.Routes["GET"].Replace("{id}", walletId.ToString()));
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                WalletResponseDto wallet = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                return await Task.FromResult(wallet);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }

        public async Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
        {
            //httpClient.DefaultRequestHeaders.Add("X-Api-Name", "StockAPI.API");

            HttpResponseMessage response = await httpClient.GetAsync(stockRoutes.Routes["GET"]
                .Replace("{date}", date)
                .Replace("{stockTicker}", stockTicker));

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Stock stock = JsonConvert.DeserializeObject<Stock>(data);
                return await Task.FromResult(stock);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }


    }
}
