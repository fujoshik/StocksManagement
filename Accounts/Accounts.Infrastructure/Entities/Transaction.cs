using Accounts.Domain.Enums;

namespace Accounts.Infrastructure.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string StockId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
