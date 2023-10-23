using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Wallet
{
    public class DepositDto
    {
        public decimal Sum { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
    }
}
