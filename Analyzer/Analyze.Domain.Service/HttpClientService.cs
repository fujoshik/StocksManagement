using Newtonsoft.Json;
using Analyzer.Domain.Abstracions.Interfaces;
using StockAPI.Infrastructure.Models;
using Analyzer.Domain.Constants;
using System.Net.Http.Json;
using Analyzer.Domain.DTOs;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(APIsConection.BaseUrl);
        }

        public async Task<WalletDto> GetAccountInfoById(Guid walletId)
        {
            string getUrl = APIsConection.GetWallet.Replace("{id}", walletId.ToString());
            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                WalletDto accountData = JsonConvert.DeserializeObject<WalletDto>(data);
                return accountData;
            }
            else
            {
                throw new HttpRequestException($"Error fetching user data. Status code: {response.StatusCode}");
            }
        }

        public async Task<Stock> GetStockData(string stockTicker, string data)
        {
            string getUrl = APIsConection.GetStock.Replace("{date}", data).Replace("{stockTicker}", stockTicker);
            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string stockData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Stock>(stockData);
            }
            else
            {
                throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
            }
        }

        public async Task<List<Stock>> GetStock(string stockTicker, string startDate, string endDate)
        {
            string apiUrl = APIsConection.GetStock.Replace("{stockTicker}", stockTicker)
                                                .Replace("{startDate}", startDate)
                                                .Replace("{endDate}", endDate);

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string stockData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Stock>>(stockData);
            }
            else
            {
                throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactions(Guid accountId, string stockTicker)
        {
            string apiUrl = APIsConection.GetTransaction + $"?accountId={accountId}&stockTicker={stockTicker}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TransactionResponseDto>>(data);
            }
            else
            {
                throw new HttpRequestException($"Error fetching transaction data. Status code: {response.StatusCode}");
            }
        }

        public async Task<SettlementDto> GetExecuteDeal(TransactionResponseDto transaction)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(APIsConection.GetSettlementAPI, transaction);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<SettlementDto>(result);
            }
            else
            {
                throw new HttpRequestException("Unsuccessful request");
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker)
        {
            string apiUrl = $"{APIsConection.GetSettlementAPI}/transactions?userId={userId}&stockTicker={stockTicker}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string transactionDataJson = await response.Content.ReadAsStringAsync();
                List<TransactionResponseDto> transactions = JsonConvert.DeserializeObject<List<TransactionResponseDto>>(transactionDataJson);

                return transactions;
            }
            else
            {
                throw new HttpRequestException($"Failed to get transactions. Status code: {response.StatusCode}");
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string stockTicker, DateTime dateTime)
        {
            string apiUrl = APIsConection.GetTransaction + $"?accountId={accountId}&stockTicker={stockTicker}&dateTime={dateTime:yyyy-MM-ddTHH:mm:ss}";

            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                List<TransactionResponseDto> transactions = JsonConvert.DeserializeObject<List<TransactionResponseDto>>(data);

                return transactions;
            }
            else
            {
                throw new HttpRequestException($"Error fetching transaction data. Status code: {response.StatusCode}");
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await httpClient.GetAsync(requestUri);
        }
    }
}
