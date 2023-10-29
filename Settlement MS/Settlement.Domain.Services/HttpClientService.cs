using Settlement.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Account;

namespace Settlement.Domain.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;

        public HttpClientService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task<UserAccountInfoDto> GetUserAccountBalance(string userId)
        {
            UserAccountInfoDto info = new UserAccountInfoDto();

            if(userId == "example")
            {
                info.InitialBalance = 1000M;
                info.CurrentBalance = 1000M;
                return Task.FromResult(info);
            }
            else
            {
                info.InitialBalance = 0M;
                info.CurrentBalance = 0M;
                return Task.FromResult(info);
            }
        }
    }
}
