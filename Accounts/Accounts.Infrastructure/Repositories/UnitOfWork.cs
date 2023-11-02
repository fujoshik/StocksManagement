using Accounts.Domain.Abstraction.Repositories;
using Microsoft.Extensions.Configuration;

namespace Accounts.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly IConfiguration _configuration;
        private IAccountRepository _accountRepository;
        private IUserRepository _userRepository;
        private IWalletRepository _walletRepository;
        private ITransactionRepository _transactionRepository;

        public UnitOfWork(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_configuration);
                }
                return _accountRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_configuration);
                }
                return _userRepository;
            }
        }

        public IWalletRepository WalletRepository
        {
            get
            {
                if (_walletRepository == null)
                {
                    _walletRepository = new WalletRepository(_configuration);
                }
                return _walletRepository;
            }
        }

        public ITransactionRepository TransactionRepository
        {
            get
            {
                if (_transactionRepository == null)
                {
                    _transactionRepository = new TransactionRepository(_configuration);
                }
                return _transactionRepository;
            }
        }
    }
}
