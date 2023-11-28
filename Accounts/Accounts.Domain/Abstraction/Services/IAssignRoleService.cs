using Accounts.Domain.DTOs.Wallet;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAssignRoleService
    {
        int AssignRole(DepositDto deposit, WalletResponseDto wallet = null);
    }
}
