namespace Accounts.Infrastructure.Entities
{
    public class Wallet : BaseEntity
    {
        public Guid AccountId { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public string CurrencyCode { get; set; }
    }
}
