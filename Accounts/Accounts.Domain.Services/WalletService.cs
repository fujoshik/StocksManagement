using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;

namespace Accounts.Domain.Services
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserDetailsProvider _userDetailsProvider;
        private readonly ICurrencyConverterService _currencyConverterService;
        private readonly IAssignRoleService _assignRoleService;

        public WalletService(IUnitOfWork unitOfWork,
                             IUserDetailsProvider userDetailsProvider,
                             ICurrencyConverterService currencyConverterService,
                             IAssignRoleService assignRoleService)
        {
            _unitOfWork = unitOfWork;
            _userDetailsProvider = userDetailsProvider;
            _currencyConverterService = currencyConverterService;
            _assignRoleService = assignRoleService;
        }

        public async Task<WalletResponseDto> CreateAsync(WalletRequestDto wallet)
        {
            if (wallet.InitialBalance == 0)
            {
                wallet.CurrentBalance = 10000;
            }
            
            return await _unitOfWork.WalletRepository.InsertAsync<WalletRequestDto, WalletResponseDto>(wallet);
        }

        public async Task<WalletResponseDto> GetWalletInfoAsync(Guid id)
        {
            if (id == default)
            {
                id = _userDetailsProvider.GetAccountId();
            }

            return await _unitOfWork.WalletRepository.GetByIdAsync<WalletResponseDto>(id);
        }

        public async Task DepositSumAsync(DepositDto deposit)
        {
            var accountId = _userDetailsProvider.GetAccountId();
            var currentCurrency = await _unitOfWork.WalletRepository.GetCurrencyCodeAsync(accountId);

            deposit.Sum = _currencyConverterService.Convert(currentCurrency, deposit.CurrencyCode, deposit.Sum);

            await ChangeRoleBasedOnBalance(accountId, deposit);

            await _unitOfWork.WalletRepository.DepositSumAsync(_userDetailsProvider.GetAccountId(), deposit);
        }

        public async Task ChangeCurrencyAsync(CurrencyCode currency)
        {
            var accountId = _userDetailsProvider.GetAccountId();
            var currentCurrency = await _unitOfWork.WalletRepository.GetCurrencyCodeAsync(accountId);
            var initialBalance = await _unitOfWork.WalletRepository.GetBalanceAsync(accountId, "InitialBalance");
            var currentBalance = await _unitOfWork.WalletRepository.GetBalanceAsync(accountId, "CurrentBalance");

            if (currency != currentCurrency)
            {
                var newInitialBalance = _currencyConverterService.Convert(currency, currentCurrency, initialBalance);
                var newCurrentBalance = _currencyConverterService.Convert(currency, currentCurrency, currentBalance);

                await _unitOfWork.WalletRepository
                    .ChangeCurrencyCodeAsync(accountId, (int)currency, newInitialBalance, newCurrentBalance);
            }
        }

        private async Task ChangeRoleBasedOnBalance(Guid accountId, DepositDto deposit)
        {
            var account = await _unitOfWork.AccountRepository.GetByIdAsync<AccountResponseDto>(accountId);
            var wallet = await _unitOfWork.WalletRepository.GetByIdAsync<WalletResponseDto>(account.WalletId);
            int role = _assignRoleService.AssignRole(deposit, wallet);

            if (account.Role == Role.Trial && role != (int) Role.Trial)
            {
                await _unitOfWork.AccountRepository.DeleteDateToDeleteAsync(accountId);
            }
            await _unitOfWork.AccountRepository.UpdateRoleAsync(accountId, role);
        }
    }
}
