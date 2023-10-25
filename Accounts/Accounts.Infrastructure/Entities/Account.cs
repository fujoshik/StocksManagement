namespace Accounts.Infrastructure.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Guid WalletId { get; set; }
    }
}
