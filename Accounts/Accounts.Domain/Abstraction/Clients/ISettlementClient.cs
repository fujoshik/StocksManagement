using Accounts.Domain.DTOs.Settlement;
using Accounts.Domain.DTOs.Transaction;

namespace Accounts.Domain.Abstraction.Clients
{
    public interface ISettlementClient
    {
        HttpClient GetSettlementClient();
        Task<SettlementResponseDto> ExecuteDeal(TransactionForSettlementDto transactionRequest);
    }
}
