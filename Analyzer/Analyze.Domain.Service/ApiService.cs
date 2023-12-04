using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;
using Accounts.Domain.DTOs.Transaction;
using Analyzer.Domain.Constants;
using Analyzer.Domain.DTOs;

namespace Analyze.Domain.Service
{
    public class ApiService : IService
    {
        private readonly IHttpClientService httpClientAccaounts;
        // private readonly IHttpClientService httpClientSettlement;

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
                    // Handle the error or throw an exception
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
                    // Handle the error or throw an exception
                    throw new HttpRequestException($"Error fetching stock data. Status code: {response.StatusCode}");
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

    }
}
