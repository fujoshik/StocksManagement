using Gateway.Domain.DTOs.Trade;

namespace Gateway.Infrastructure.Entities
{
    public class Transaction : BaseEntity
    {
        public int Id { get; set; }
        public User User { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        
    }

}
