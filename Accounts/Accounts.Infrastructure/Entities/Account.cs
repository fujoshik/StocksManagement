using Accounts.Domain.Enums;

namespace Accounts.Infrastructure.Entities
{
    public class Account : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
        public DateTime DateToDelete { get; set; }
        public Guid WalletId { get; set; }
    }
}
