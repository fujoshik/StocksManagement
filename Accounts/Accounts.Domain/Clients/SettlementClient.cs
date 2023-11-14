using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Settings;

namespace Accounts.Domain.Clients
{
    public class SettlementClient : ISettlementClient
    {
        private HttpClient _httpClient;
        private readonly string _settlementApiUrl;

        public SettlementClient(HostsSettings hosts)
        {
            _settlementApiUrl = hosts.Settlement;
        }

        public HttpClient GetSettlementClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_settlementApiUrl)
            };
            return _httpClient;
        }
    }
}
