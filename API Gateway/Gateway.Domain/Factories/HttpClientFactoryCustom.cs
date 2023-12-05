using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.Abstraction.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Factories
{
    public class HttpClientFactoryCustom : IHttpClientFactoryCustom
    {
        private readonly IAccountClient _accountClient;

        public HttpClientFactoryCustom(IAccountClient accountClient)
        {
            _accountClient = accountClient;
        }

        public IAccountClient GetAccountClient()
        {
            return _accountClient;
        }
    }
}
