using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.Abstraction.Clients.AccountClient;

namespace Gateway.Domain.Abstraction.Factories
{
    public interface IHttpClientFactoryCustom
    {
        IAccountClient GetAccountClient();
        IStockClient GetStockClient();
        ISettlementsClient GetSettlementsClient();
        IAnalyzerClient GetAnalyzerClient();
    }
}
