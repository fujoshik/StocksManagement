using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StockAPI.Infrastructure.Models;

namespace Accounts.Domain.Clients
{
    public class StockApiClient : IStockApiClient
    {
        private HttpClient _httpClient;
        private readonly string _stockApiUrl;

        public StockApiClient(IOptions<HostsSettings> hosts)
        {
            _stockApiUrl = hosts.Value.StockApi;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_stockApiUrl)
            };
        }

        public HttpClient GetStockApiClient()
        {
            return _httpClient;
        }

        public async Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
        {
            var response = await _httpClient.GetAsync(_stockApiUrl + "get-stocks-by-date");

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Stock>(result);
        }
    }
}
