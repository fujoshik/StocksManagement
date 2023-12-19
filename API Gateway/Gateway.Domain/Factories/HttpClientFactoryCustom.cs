using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.Abstraction.Factories;
namespace Gateway.Domain.Factories
{
    public class HttpClientFactoryCustom : IHttpClientFactoryCustom
    {
        private readonly IAccountClient _accountClient;
        private readonly ISettlementsClient _settlementsClient;
        private readonly IStockClient _stockClient;
        private readonly IAnalyzerClient _analyzerClient;

        public HttpClientFactoryCustom(IAccountClient accountClient,
                                       ISettlementsClient settlementsClient,
                                       IStockClient stockClient,
                                       IAnalyzerClient analyzerClient)
        {
            _accountClient = accountClient;
            _settlementsClient = settlementsClient;
            _stockClient = stockClient;
            _analyzerClient = analyzerClient;
        }

        public IAccountClient GetAccountClient()
        {
            return _accountClient;
        }

        public IAnalyzerClient GetAnalyzerClient()
        {
            return _analyzerClient;
        }

        public ISettlementsClient GetSettlementsClient()
        {
            return _settlementsClient;
        }

        public IStockClient GetStockClient()
        {
            return _stockClient;
        }
    }
}
