using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Wallet
{
    public class WalletRequestDto
    {
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public int CurrencyCode { get; set; }
    }
}
