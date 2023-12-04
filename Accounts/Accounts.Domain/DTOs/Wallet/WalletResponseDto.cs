using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Wallet
{
    public class WalletResponseDto : BaseResponseDto
    {
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public object UserData { get; set; }
    }
}
