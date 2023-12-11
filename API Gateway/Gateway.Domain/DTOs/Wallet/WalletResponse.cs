using Gateway.Domain.Enums;

namespace Gateway.Domain.DTOs.Wallet
{
    public class WalletResponse
    {
        public Guid Id { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public Currency Currency { get; set; }
    }
}
