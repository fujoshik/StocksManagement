namespace Accounts.Domain.DTOs.Account
{
    public class AccountResponseDto : BaseResponseDto
    {
        public string Email { get; set; }
        public Guid WalletId { get; set; }
    }
}
