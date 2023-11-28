using Gateway.Domain.Abstraction.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Factories
{
    public interface IHttpClientFactory
    {
        IAccountClient GetAccountClient();
    }
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IAccountClient _accountClient;

        public HttpClientFactory(IAccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        public IAccountClient GetAccountClient()
        {
            return _accountClient;
        }
    }
}
