using Accounts.Domain.Enums;

namespace Accounts.Infrastructure.Entities
{
    public class Wallet : BaseEntity
    {
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
    }
}
