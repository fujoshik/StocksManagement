using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.DTOs.Settlement;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Accounts.Domain.Clients
{
    public class AnalyzerClient : IAnalyzerClient
    {
        private HttpClient _httpClient;
        private readonly string _analyzerApiUrl;

        public AnalyzerClient(IOptions<HostsSettings> hosts)
        {
            _analyzerApiUrl = hosts.Value.Analyzer;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_analyzerApiUrl)
            };
        }

        public HttpClient GetAnalyzerClient()
        {
            return _httpClient;
        }

        public async Task<decimal> CalculateAverageIncomeAsync(Guid accountId, string ticker)
        {   
            var query = $"?userId={accountId}&stockTicker={ticker}";
            var response = await _httpClient.GetAsync(_analyzerApiUrl + "calculate-current-yield" + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<decimal>(result);
        }
    }
}
