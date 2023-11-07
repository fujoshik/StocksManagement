namespace Accounts.Domain.Abstraction.Clients
{
    public interface ISettlementClient
    {
        HttpClient GetSettlementClient();
    }
}
