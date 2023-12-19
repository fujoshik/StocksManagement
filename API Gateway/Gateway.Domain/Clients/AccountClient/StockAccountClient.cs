using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.DTOs.Stock;
using Gateway.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients.AccountClient
{
    public class StockAccountClient : BaseAccountClient, IStockAccountClient
    {
        public StockAccountClient(IOptions<HostSettings> hostSettings,
                                  IOptions<AccountSettings> accountSettings,
                                  IOptions<UserSettings> userSettings,
                                  IHttpContextAccessor httpContextAccessor) 
            : base(hostSettings, accountSettings, userSettings, httpContextAccessor)
        {
        }
        public async Task BuyStockAsync(BuyStockDTO buyStock)
        {
            AddAuthorizationHeader();

            var query = string.Format($"?ticker={buyStock.Ticker}&quantity={buyStock.Quantity}");

            var response = await _httpClient.PostAsync(_accountApiUrl + _accountSettings.BuyStockRoute + query, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task SellStockAsync(BuyStockDTO sellStock)
        {
            AddAuthorizationHeader();

            var query = string.Format($"?ticker={sellStock.Ticker}&quantity={sellStock.Quantity}");

            var response = await _httpClient.PostAsync(_accountApiUrl + _accountSettings.SellStockRoute + query, null);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task<decimal> CalculateAverageIncomeAsync(string stockTicker, string date)
        {
            AddAuthorizationHeader();

            var query = string.Format($"?stockTicker={stockTicker}&date={date}");

            var response = await _httpClient.GetAsync(_accountApiUrl + _accountSettings.CalculateAverageIncomeRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<decimal>();
        }
    }
}
