using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;

namespace Accounts.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
