using Accounts.Domain.Abstraction.Factories;
using Accounts.Domain.Settings;

namespace Accounts.Domain.Factories
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string _stockApiUrl;
        private readonly string _settlementApiUrl;
        private HttpClient _httpClient;

        public HttpClientFactory(HostsSettings hosts)
        {
            _stockApiUrl = hosts.StockApi;
            _settlementApiUrl = hosts.Settlement;
            _httpClient = new HttpClient();
        }
        public HttpClient GetStockApiClient()
        { 
            _httpClient.BaseAddress = new Uri(_stockApiUrl);
            return _httpClient;
        }

        public HttpClient GetSettlementClient()
        {
            _httpClient.BaseAddress = new Uri(_settlementApiUrl);
            return _httpClient;
        }
    }
}
