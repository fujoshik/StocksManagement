using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Transaction
{
    public class TransactionForSettlementDto
    {
        public Guid WalletId { get; set; }
        public string Date { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public Guid AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
