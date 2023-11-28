using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Transaction
{
    public class TransactionRequestDto
    {
        public Guid AccountId { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid WalletId { get; set; }
    }
}
