using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.ExecuteDeal
{
    public class ExecuteDealDto
    {
        public string Ticker { get; set; }
        public TransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public Guid WalletId { get; set; }
        public Guid AccountId { get; set; }
    }
}
