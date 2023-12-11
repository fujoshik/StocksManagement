using Gateway.Domain.Enums;

namespace Gateway.Domain.DTOs.Wallet
{
    public class DepositSumDto
    {
        public decimal Sum { get; set; }
        public Currency Currency { get; set; }
    }
}
