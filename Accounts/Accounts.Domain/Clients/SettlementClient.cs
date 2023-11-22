using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.DTOs.Settlement;
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Accounts.Domain.Clients
{
    public class SettlementClient : ISettlementClient
    {
        private HttpClient _httpClient;
        private readonly string _settlementApiUrl;

        public SettlementClient(IOptions<HostsSettings> hosts)
        {
            _settlementApiUrl = hosts.Value.Settlement;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_settlementApiUrl)
            };
        }

        public HttpClient GetSettlementClient()
        {
            return _httpClient;
        }

        public async Task<SettlementResponseDto> ExecuteDeal(TransactionForSettlementDto transactionSettlement)
        {
            var response = await _httpClient.PostAsJsonAsync(_settlementApiUrl, transactionSettlement);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SettlementResponseDto>(result);
        }
    }
}
