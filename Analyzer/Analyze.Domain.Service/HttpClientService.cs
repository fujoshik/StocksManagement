using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;
using Analyzer.Domain.Constants;
using System.Net.Http.Json;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient accountClient;
        private readonly HttpClient stockApi;
        private readonly HttpClient settlementApi;
        private readonly HttpClient settlementApiDetails;

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
        }

        public async Task<WalletDto> GetAccountInfoById(Guid id)
        {
            using (var httpClient = GetAccountClient())
            {
                string getUrl = $"/accounts-api/wallets/{id}";
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



        /*public async Task<List<TransactionResponseDto>> GetTransactions([FromBody] Guid walletId)
        {
            try
            {
                string apiUrl = $"{APIsConection.GetSettlementAPI}/transactions";

                // Assuming you need to send the walletId in the request body
                var requestData = new { WalletId = walletId };
                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await settlementApi.PostAsync(apiUrl, content);

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
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception: {ex.Message}");
                throw;
            }
        }*/

        public async Task<SettlementDto> GetTransactions(TransactionResponseDto transaction)
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = APIsConection.GetSettlementAPI;
                var response = await httpClient.PostAsJsonAsync(apiUrl, transaction);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error content: {errorContent}");
                    throw new HttpRequestException("Unsuccessful request");
                }
                //if (!response.IsSuccessStatusCode)
                //{
                //    throw new HttpRequestException("Unsuccessful request");
                //}

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<SettlementDto>(result);
            }
        }



        public async Task<List<TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker)
        {
            try
            {
                using (var httpClient = GetSettlementAPI())
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


        public HttpClient GetAccountClient()
        {
            return accountClient;
        }

        public HttpClient GetStockAPI()
        {
            return stockApi;
        }

        public HttpClient GetSettlementAPI()
        {
            return settlementApi;
        }

        public HttpClient GetTransactionsDetails()
        {
            return settlementApiDetails;
        }
    }
}

