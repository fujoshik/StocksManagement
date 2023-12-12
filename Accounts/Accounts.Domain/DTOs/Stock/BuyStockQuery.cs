namespace Accounts.Domain.DTOs.Stock
{
    public class BuyStockQuery
    {
        public string Ticker { get; set; }
        public int Quantity { get; set; }
    }
}
