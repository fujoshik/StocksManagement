using Newtonsoft.Json;
using Analyzer.Domain.Abstracions.Interfaces;
using StockAPI.Infrastructure.Models;
using Accounts.Domain.DTOs.Transaction;
using Analyzer.Domain.Constants;
using Analyzer.Domain.DTOs;

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

        public async Task<Stock> GetStockData(string stockTicker, string Data)
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
        public async Task<List<Analyzer.Domain.DTOs.TransactionResponseDto>> GetTransactions(Guid walletId)
        {
            try
            {
                using (var httpClient = httpClientAccaounts.GetSettlementAPI())
                {
                    string apiUrl = $"{APIsConection.GetSettlementAPI}/transactions?walletId={walletId}";

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string transactionDataJson = await response.Content.ReadAsStringAsync();
                        List<Analyzer.Domain.DTOs.TransactionResponseDto> transactions = JsonConvert.DeserializeObject<List<Analyzer.Domain.DTOs.TransactionResponseDto>>(transactionDataJson);
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
                throw;
            }
        }

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
                            Price = t.Price
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






    //public async Task< UserData> GetInfoFromSettlement(string id)
    //{
    //    using (var httpClient = httpClientSettlement.GetAccountClient())
    //    {
    //        string getUrl = $"/api/accounts/{id}";
    //        HttpResponseMessage response = await httpClient.GetAsync(getUrl);

    //        if (response.IsSuccessStatusCode)
    //        {

    //        }

    //        return null;
    //    }
    //}


