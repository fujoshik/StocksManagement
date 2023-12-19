using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

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

        public async Task<decimal> CalculateAverageIncomeForPeriodAsync(Guid accountId, string ticker, string date)
        {   
            var query = $"?userId={accountId}&stockTicker={ticker}&data={date}";
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
