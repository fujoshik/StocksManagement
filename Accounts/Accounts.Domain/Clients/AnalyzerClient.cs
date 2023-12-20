using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.DTOs.Analyzer;
using Accounts.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Accounts.Domain.Clients
{
    public class AnalyzerClient : IAnalyzerClient
    {
        private HttpClient _httpClient;
        private readonly string _analyzerApiUrl;
        private readonly AnalyzerSettings _analyzerSettings;

        public AnalyzerClient(IOptions<HostsSettings> hosts,
                              IOptions<AnalyzerSettings> analyzerSettings)
        {
            _analyzerApiUrl = hosts.Value.Analyzer;
            _analyzerSettings = analyzerSettings.Value;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_analyzerApiUrl)
            };
        }

        public HttpClient GetAnalyzerClient()
        {
            return _httpClient;
        }

        public async Task<CalculateCurrentYieldDto> CalculateAverageIncomeForPeriodAsync(Guid accountId, string ticker, string date)
        {   
            var query = $"?accountId={accountId}&stockTicker={ticker}&data={date}";
            var response = await _httpClient.GetAsync(_analyzerApiUrl + _analyzerSettings.CurrentYieldRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CalculateCurrentYieldDto>(result);
        }

        public async Task<PercentageChangeDto> GetPercentageChangeAsync(Guid walletId, string ticker, string date)
        {
            var query = $"?walletId={walletId}&stockTicker={ticker}&data={date}";
            var response = await _httpClient.GetAsync(_analyzerApiUrl + _analyzerSettings.PercentageChangeRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<PercentageChangeDto>(result);
        }

        public async Task<List<DailyYieldChangeDto>> GetDailyYieldChangesAsync(string date, string ticker, Guid accountId)
        {
            var query = $"?date={date}&stockTicker={ticker}&accountId={accountId}";
            var response = await _httpClient.GetAsync(_analyzerApiUrl + _analyzerSettings.DailyYieldChangesRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<DailyYieldChangeDto>>(result);
        }
    }
}
