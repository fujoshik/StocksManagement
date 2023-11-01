using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace grupa
{
    public class ApiService : IService
    {
        private readonly HttpClient httpClient;

        public ApiService()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5234");
        }


        public async Task<UserData> GetInfoFromAccount(int id)
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

                return userData;
            }

            return null;
        }

        public async Task<string> GetUserById(int id)
        {
            string getUrl = $"/api/accounts/{id}";

            HttpResponseMessage response = await httpClient.GetAsync(getUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return null;
        }



        public async Task<bool> UserProfileProfit(string UserId)
        {

            return true;
        }

    }
}
