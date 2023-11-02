namespace Accounts.Domain.Abstraction.Factories
{
    public interface IHttpClientFactory
    {
        HttpClient GetStockApiClient();
        HttpClient GetSettlementClient();
    }
}
