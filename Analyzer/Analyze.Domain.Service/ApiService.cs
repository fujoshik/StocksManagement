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
                    .Replace("{stockTicker}", stockTicker)
                    .Replace("{openPrice}",openPrice);


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

        public async Task<List<Stock>> GetStock(string stockTicker, string startDate, string endDate)
        {
            try
            {
                return await httpClientAccaounts.GetStock(stockTicker, startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error fetching stock data: {ex.Message}");
            }
        }


        public async Task<List<Analyzer.Domain.DTOs.TransactionResponseDto>> GetTransactions(Guid accountId, string stockTicker)
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
                            // Specify the full namespace for clarity
                            var transactionData = JsonConvert.DeserializeObject<List<Analyzer.Domain.DTOs.TransactionResponseDto>>(data);
                            return transactionData;
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





        //public async Task<SettlementDto> GetExecuteDeal(Analyzer.Domain.DTOs.TransactionResponseDto transaction)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var apiUrl = APIsConection.GetSettlementAPI;
        //        var response = await httpClient.PostAsJsonAsync(apiUrl, transaction);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            throw new HttpRequestException("Unsuccessful request");
        //        }

        //        var result = await response.Content.ReadAsStringAsync();

        //        return JsonConvert.DeserializeObject<SettlementDto>(result);
        //    }
        //}


        //public async Task<SettlementDto> ExecuteDealAsync(ExecuteDealDto executeDealDto)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var transactionForSettlement = new TransactionForSettlementDto()
        //        {
        //            WalletId = executeDealDto.WalletId,
        //            StockTicker = executeDealDto.Ticker,
        //            Quantity = executeDealDto.Quantity,
        //            TransactionType = executeDealDto.TransactionType,
        //            AccountId = executeDealDto.AccountId
        //        };

        //        var transactionsInstance = new GetTransactions(); 
        //        return await transactionsInstance.ExecuteDeal(transactionForSettlement);
        //    }
        //}





        public async Task<List<Analyzer.Domain.DTOs.TransactionResponseDto>> GetTransactionsDetails(Guid userId, string stockTicker)
        {
            try
            {
                using (var httpClient = httpClientAccaounts.GetSettlementAPI())
                {
                    string apiUrl = $"{APIsConection.GetSettlementAPI}/transactions?userId={userId}&stockTicker={stockTicker}";

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string transactionDataJson = await response.Content.ReadAsStringAsync();
                        List<Analyzer.Domain.DTOs.TransactionResponseDto> transactions = JsonConvert.DeserializeObject<List<Analyzer.Domain.DTOs.TransactionResponseDto>>(transactionDataJson);

                        // Extract only Quantity and Price properties
                        var transactionsDetails = transactions.Select(t => new Analyzer.Domain.DTOs.TransactionResponseDto
                        {
                            Quantity = t.Quantity, 
                        }).ToList();

                        return transactionsDetails;
                    }
                    else
                    {
                        throw new HttpRequestException($"Failed to get transactions. Status code: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}