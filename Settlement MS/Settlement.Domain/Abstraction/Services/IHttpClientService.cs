using Settlement.Domain.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IHttpClientService
    {
        Task<UserAccountInfoDto> GetUserAccountBalance(string userId);
        public HttpClient GetAccountClient();
    }
}
