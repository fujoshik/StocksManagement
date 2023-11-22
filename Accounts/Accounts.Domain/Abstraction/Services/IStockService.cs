namespace Accounts.Domain.Abstraction.Services
{
    public interface IStockService
    {
        Task BuyStockAsync(string ticker, int quantity);
    }
}
