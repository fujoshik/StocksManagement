using Accounts.Domain.DTOs.ExecuteDeal;
using Accounts.Domain.DTOs.Settlement;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Abstraction.Services
{
    public interface ISettlementService
    {
        Task<SettlementResponseDto> ExecuteDealAsync(ExecuteDealDto executeDealDto);
    }
}
