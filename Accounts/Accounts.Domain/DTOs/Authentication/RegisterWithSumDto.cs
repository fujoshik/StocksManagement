using Accounts.Domain.Enums;

namespace Accounts.Domain.DTOs.Authentication
{
    public class RegisterWithSumDto : RegisterDto
    {
        public decimal Sum { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
    }
}
