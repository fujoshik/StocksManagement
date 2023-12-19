using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.DTOs.Stock;
using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients
{
    public class StockClient : IStockClient
    {
        private HttpClient _httpClient;
        private readonly string _stockApiUrl;
        private readonly StockApiSettings _stockApiSettings;
        public StockClient(IOptions<HostSettings> hostSettings,
                           IOptions<StockApiSettings> stockSettings)
        {
            _stockApiUrl = hostSettings.Value.StockApi;
            _stockApiSettings = stockSettings.Value;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_stockApiUrl)
            };
        }

        public HttpClient GetApi()
        {
            return _httpClient;
        }

        public async Task<List<StockDTO>> GetGroupedDaily()
        {
            var response = await _httpClient.GetAsync(_stockApiUrl + _stockApiSettings.GetGroupedDailyRoute);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<StockDTO>>(json);
        }

        public async Task<StockDTO> GetStockByDateAndTickerFromAPI(string date, string ticker)
        {
            var query = string.Format($"?date={date}&stockTicker={ticker}");

            var response = await _httpClient.GetAsync(_stockApiUrl + _stockApiSettings.GetStockByDateAndTickerFromAPIRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<StockDTO>();
        }

        public async Task<StockDTO> GetStockByDateAndTicker(string date, string ticker)
        {
            var query = string.Format($"?date={date}&stockTicker={ticker}");

            var response = await _httpClient.GetAsync(_stockApiUrl + _stockApiSettings.GetStockByDateAndTickerRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<StockDTO>();
        }

        public async Task<List<StockDTO>> GetStocksByDate(string date)
        {
            var query = string.Format($"?date={date}");

            var response = await _httpClient.GetAsync(_stockApiUrl + _stockApiSettings.GetStocksByDateRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<StockDTO>>(json);
        }

        public async Task<StockMarketCharacteristicsDTO> GetStockMarketCharacteristics(string date)
        {
            var query = string.Format($"?date={date}");

            var response = await _httpClient.GetAsync(_stockApiUrl + _stockApiSettings.GetStockMarketCharacteristicsRoute + query);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            //var stringResponse = await response.Content.ReadAsStringAsync();

            //var json = JsonConvert.DeserializeObject<object>(stringResponse);

            //return JsonConvert.DeserializeObject<StockMarketCharacteristicsDTO>(json.ToString());

            return await response.Content.ReadFromJsonAsync<StockMarketCharacteristicsDTO>();
        }
    }
}
