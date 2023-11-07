using Accounts.Domain.Abstraction.Clients;

namespace Accounts.Domain.Abstraction.Factories
{
    public interface IHttpClientFactory
    {
        IStockApiClient StockApiClient();
        ISettlementClient SettlementClient();
    }
}
