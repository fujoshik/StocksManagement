using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Services
{
    public class DeleteWhenTrialEndsService : IDeleteWhenTrialEndsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWhenTrialEndsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAccountWhenTrialEndsAsync(Guid accountId, string dateToDelete)
        {
            if (dateToDelete != null)
            {
                if (DateTime.Parse(dateToDelete) <= DateTime.Now)
                {
                    var account = await _unitOfWork.AccountRepository.GetByIdAsync<AccountResponseDto>(accountId);
                    await _unitOfWork.UserRepository.DeleteByAccountIdAsync(accountId);
                    await _unitOfWork.TransactionRepository.DeleteByAccountIdAsync(accountId);
                    await _unitOfWork.AccountRepository.DeleteAsync(accountId);
                    await _unitOfWork.WalletRepository.DeleteAsync(account.WalletId);

                    return false;
                }

                if (DateTime.Parse(dateToDelete) == DateTime.UtcNow.AddMonths(1))
                {
                    await _unitOfWork.AccountRepository.UpdateRoleAsync(accountId, (int)Role.Inactive);
                }
            }
            return true;
        }
    }
}
