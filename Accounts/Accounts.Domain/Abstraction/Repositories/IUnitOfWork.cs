namespace Accounts.Domain.Abstraction.Repositories
{
    public interface IUnitOfWork
    {
        public IAccountRepository AccountRepository { get; }
        public IUserRepository UserRepository { get; }
        public IWalletRepository WalletRepository { get; }
    }
}
