using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDetailsProvider _userDetailsProvider;

        public TransactionService(IUnitOfWork unitOfWork,
                                  IUserDetailsProvider userDetailsProvider)
        {
            _unitOfWork = unitOfWork;
            _userDetailsProvider = userDetailsProvider;
        }

        public async Task<List<TransactionResponseDto>> GetSoldTransactionsByAccountAsync(Guid accountId)
        {
            return await _unitOfWork.TransactionRepository.GetSoldTransactionsByAccountId(accountId);
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsByAccountIdAndTickerAsync(Guid accountId, string ticker)
        {
            return await _unitOfWork.TransactionRepository.GetTransactionsByAccountIdAndTickerAsync(accountId, ticker);
        }
    }
}
