namespace Accounts.Domain.Abstraction.Services
{
    public interface IStockService
    {
        Task BuyStockAsync(string ticker, int quantity);
        Task SellStockAsync(string ticker, int quantity);
    }
}
