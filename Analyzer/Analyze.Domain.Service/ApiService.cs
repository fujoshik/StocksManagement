using Newtonsoft.Json;
using Analyzer.Domain.Abstracions.Interfaces;
using StockAPI.Infrastructure.Models;
using Accounts.Domain.DTOs.Transaction;
using Analyzer.Domain.Constants;
using Analyzer.Domain.DTOs;
using System.Net.Http.Json;
using System.Net.Http;
using Accounts.Domain.DTOs.ExecuteDeal;

namespace Analyze.Domain.Service
{
    public class ApiService : IService
    {
        private readonly IHttpClientService httpClientAccaounts;

        public ApiService(IHttpClientService httpClientAccaounts)
        {
            this.httpClientAccaounts = httpClientAccaounts;
        }

        public async Task<WalletDto> GetAccountInfoById(Guid id)
        {
            using (var httpClient = httpClientAccaounts.GetAccountClient())
            {
                string getUrl = APIsConection.GetWallet.Replace("{id}", id.ToString());
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

        public async Task<Stock> GetStockData(string stockTicker, string Data, string openPrice)
        {
            using (var httpClient = httpClientAccaounts.GetStockAPI())
            {
                string getUrl = APIsConection.GetStock
                    .Replace("{date}", Data)
                    .Replace("{stockTicker}", stockTicker);


                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    Stock stockData = JsonConvert.DeserializeObject<Stock>(Data);
                    return stockData;
                }
                else
                {
                    throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
                }
            }
        }




        public async Task<Analyzer.Domain.DTOs.TransactionResponseDto> GetTransactions(string stockTicker)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var apiUrl = APIsConection.GetTransaction;
                    var queryParameters = $"?stockTicker={stockTicker}";

                    apiUrl += queryParameters;

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(data))
                        {
                            Analyzer.Domain.DTOs.TransactionResponseDto transactionData = JsonConvert.DeserializeObject<Analyzer.Domain.DTOs.TransactionResponseDto>(data);

                            if (transactionData != null)
                            {
                                return transactionData;
                            }
                            else
                            {
                                throw new InvalidOperationException("Deserialization resulted in a null TransactionResponseDto.");
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException("Received data is empty or null.");
                        }
                    }
                    else
                    {
                        throw new HttpRequestException($"Error fetching transaction data. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching or deserializing transaction data: {ex.Message}");
            }
        }
    }
}


