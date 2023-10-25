using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Wallet
{
    public class WalletResponseDto : BaseResponseDto
    {
        public decimal CurrentBalance { get; set; }
        public CurrencyCode Currency { get; set; }
    }
}
