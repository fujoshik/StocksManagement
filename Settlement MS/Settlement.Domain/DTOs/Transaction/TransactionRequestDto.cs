using Settlement.Domain.Enums;

namespace Settlement.Domain.DTOs.Transaction
{
    public class TransactionRequestDto : BaseDto
    {
        public Guid WalletId { get; set; }
        public string Date { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public Guid AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal StockPrice { get; set; }
        public RoleType Role { get; set; }
    }
}
