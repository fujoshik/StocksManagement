using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class ApiService : IService
    {
        private readonly IHttpClientService httpClientAccaounts;
        private readonly IHttpClientService httpClientSettlement;

        public ApiService(IHttpClientService httpClientAccaounts)
        {
            this.httpClientAccaounts = httpClientAccaounts;
        }

        public async Task<WalletResponseDto> GetAccountInfoById(Guid id)
        {
            using (var httpClient = httpClientAccaounts.GetAccountClient())
            {
                string getUrl = $"/accounts-api/wallets/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    WalletResponseDto accountData = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                    return accountData;
                }

                return null;
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
