using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TransactionResponseDto>> GetSoldTransactionsByAccountAsync(Guid accountId)
        {
            return await _unitOfWork.TransactionRepository.GetSoldTransactionsByAccountId(accountId);
        }
    }
}
