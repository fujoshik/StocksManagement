using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;

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


        public async Task<UserData> GetInfoFromAccount(int id)
        {
            using (var httpClient = httpClientAccaounts.GetAccountClient())
            {
                string getUrl = $"/api/accounts/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);


                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jsonData = JObject.Parse(data);

                    UserData userData = new UserData();

                    if (jsonData.TryGetValue("AccountGuid", out var accountGuidToken))
                    {
                        userData.AccountGuid = Guid.Parse(accountGuidToken.ToString());
                    }

                    if (jsonData.TryGetValue("Amount", out var amountToken))
                    {
                        userData.Amount = decimal.Parse(amountToken.ToString());
                    }

                    if (jsonData.TryGetValue("Amount", out var currentBalanceToken))
                    {
                        userData.CurrentBalance = decimal.Parse(currentBalanceToken.ToString());
                    }

                    return userData;
                }

                return null;
            }
        }

        //public async Task<string> GetUserById(int id)
        //{
        //    string getUrl = $"/api/accounts/{id}";

        //    HttpResponseMessage response = await httpClientAccaounts.GetAsync(getUrl);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        string data = await response.Content.ReadAsStringAsync();
        //        return data;
        //    }

        //    return null;
        //}



        public async Task<UserData> GetInfoFromSettlement(string id)
        {
            using (var httpClient = httpClientSettlement.GetAccountClient())
            {
                string getUrl = $"/api/accounts/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(getUrl);

                if (response.IsSuccessStatusCode)
                {

                }

                return null;
            }
        }

    }
}
