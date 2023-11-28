using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Services
{
    public class AssignRoleService : IAssignRoleService
    {
        private readonly ICurrencyConverterService _currencyConverterService;

        public AssignRoleService(ICurrencyConverterService currencyConverterService)
        {
            _currencyConverterService = currencyConverterService;
        }

        public int AssignRole(DepositDto deposit, WalletResponseDto wallet = null)
        {
            decimal sum = deposit.Sum;          

            if (deposit.CurrencyCode != CurrencyCode.USD)
            {
                sum = _currencyConverterService.Convert(deposit.CurrencyCode, CurrencyCode.USD, deposit.Sum);
            }

            if (wallet != null)
            {
                if (wallet.CurrencyCode != CurrencyCode.USD)
                {
                    sum += _currencyConverterService.Convert(wallet.CurrencyCode, CurrencyCode.USD, wallet.CurrentBalance);
                }
                sum += wallet.CurrentBalance;
            }

            if (sum > 0)
            {
                if (sum < 1000)
                    return (int)Role.Regular;
                if (sum >= 1000 && sum < 5000)
                    return (int)Role.Special;
                if (sum >= 5000)
                    return (int)Role.VIP;
            }
            return (int)Role.Trial;
        }
    }
}
