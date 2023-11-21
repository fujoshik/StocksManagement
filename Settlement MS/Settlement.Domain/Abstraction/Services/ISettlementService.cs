using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Abstraction.Services
{
    public interface ISettlementService
    {
        Task<SettlementResponseDto> ExecuteDeal(TransactionRequestDto transactionRequest);
    }
}
