using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Settings;

namespace Accounts.Domain.Clients
{
    public class StockApiClient : IStockApiClient
    {
        private HttpClient _httpClient;
        private readonly string _stockApiUrl;

        public StockApiClient(HostsSettings hosts)
        {
            _stockApiUrl = hosts.StockApi;
        }

        public HttpClient GetStockApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_stockApiUrl)
            };
            return _httpClient;
        }
    }
}
