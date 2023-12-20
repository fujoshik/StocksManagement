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
        private readonly HttpClient accountClient;
        private readonly HttpClient stockApi;
        private readonly HttpClient settlementApi;
        private readonly HttpClient settlementApiDetails;
        private readonly HttpClient stock;

        public HttpClientService()
        {
            accountClient = new HttpClient();
            accountClient.BaseAddress = new Uri(APIsConection.GetWallet);

            stockApi = new HttpClient();
            stockApi.BaseAddress = new Uri(APIsConection.GetStock);

            settlementApi = new HttpClient();
            settlementApi.BaseAddress = new Uri(APIsConection.GetSettlementAPI);

            settlementApiDetails = new HttpClient();
            settlementApiDetails.BaseAddress = new Uri(APIsConection.GetTransactionsDetails);

            stock = new HttpClient();
            stock.BaseAddress = new Uri(APIsConection.GetStock);
        }

        public async Task<WalletDto> GetAccountInfoById(Guid walletId)
        {
            using (var httpClient = GetAccountClient())
            {
                string getUrl = $"/accounts-api/wallets/{walletId}";
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
        }

        public async Task<Stock> GetStockData(string stockTicker, string data)
        {
            using (var httpClient = GetStockAPI())
            {
                string getUrl = $"/api/StockAPI/get-stock-by-date-and-ticker?date={data}&stockTicker={stockTicker}";
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
        }

        public async Task<List<Stock>> GetStock(string stockTicker, string startDate, string endDate)
        {
            using (var httpClient = GetHttpClient())
            {
                try
                {
                    string apiUrl = $"/api/StockAPI/get-stock-by-date-and-ticker?stockTicker={stockTicker}&startDate={startDate}&endDate={endDate}";

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
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error fetching stock data: {ex.Message}");
                }
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactions(Guid accountId, string stockTicker)
        {
            try
            {
                using (var httpClient = GetTransactions())
                {
                    var apiUrl = APIsConection.GetTransaction;
                    var queryParameters = $"?accountId={accountId}&stockTicker={stockTicker}";

                    apiUrl += queryParameters;

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        var transactionData = JsonConvert.DeserializeObject<List<TransactionResponseDto>>(data);
                        return transactionData;
                    }
                    else
                    {
                        throw new HttpRequestException($"Error fetching transaction data. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<SettlementDto> GetExecuteDeal(TransactionResponseDto transaction)
        {
            using (var httpClient = GetSettlementAPI())
            {
                var apiUrl = APIsConection.GetSettlementAPI;
                var response = await httpClient.PostAsJsonAsync(apiUrl, transaction);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Unsuccessful request");
                }

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SettlementDto>(result);
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker)
        {
            try
            {
                using (var httpClient = GetTransactionsDetails())
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
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string stockTicker, DateTime dateTime)
        {
            try
            {
                using (var httpClient = GetTransactions())
                {
                    var apiUrl = APIsConection.GetTransaction;
                    var queryParameters = $"?accountId={accountId}&stockTicker={stockTicker}&dateTime={dateTime:yyyy-MM-ddTHH:mm:ss}";

                    apiUrl += queryParameters;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }

        private HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            return httpClient;
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            using (var httpClient = GetHttpClient())
            {
                return await httpClient.GetAsync(requestUri);
            }
        }

        public HttpClient GetAccountClient()
        {
            return accountClient;
        }

        public HttpClient GetStockAPI()
        {
            return stockApi;
        }

        public HttpClient GetStock()
        {
            return stock;
        }

        public HttpClient GetSettlementAPI()
        {
            return settlementApi;
        }

        public HttpClient GetTransactionsDetails()
        {
            return settlementApiDetails;
        }

        public HttpClient GetTransactions()
        {
            return settlementApiDetails;
        }
    }
}