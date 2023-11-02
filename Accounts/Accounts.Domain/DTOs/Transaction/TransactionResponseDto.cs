using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Transaction
{
    public class TransactionResponseDto : BaseResponseDto
    {
        public Guid AccountId { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
