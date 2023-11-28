using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.ExecuteDeal;
using Accounts.Domain.Enums;
using Accounts.Domain.Exceptions;

namespace Accounts.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IUserDetailsProvider _userDetailsProvider;
        private readonly IAccountService _accountService;
        private readonly ISettlementService _settlementService;
        private readonly IWalletService _walletService;

        public StockService(IUserDetailsProvider userDetailsProvider,
                            IAccountService accountService,
                            ISettlementService settlementService,
                            IWalletService walletService)
        {
            _userDetailsProvider = userDetailsProvider;
            _accountService = accountService;
            _settlementService = settlementService;
            _walletService = walletService;
        }

        public async Task BuyStockAsync(string ticker, int quantity)
        {
            var currentAccount = await _accountService.GetByIdAsync(_userDetailsProvider.GetAccountId());

            var wallet = await _walletService.GetWalletInfoAsync(currentAccount.WalletId);
            CurrencyCode beforeCurrency = wallet.CurrencyCode;

            if (wallet.CurrencyCode != CurrencyCode.USD)
            {
                await _walletService.ChangeCurrencyAsync(CurrencyCode.USD);
            }

            //add Role and calculate based on the type of client
            var result = await _settlementService.ExecuteDealAsync(new ExecuteDealDto
            {
                Ticker = ticker,
                Quantity = quantity,
                TransactionType = TransactionType.Bought,
                AccountId = currentAccount.Id,
                WalletId = currentAccount.WalletId
            });

            if (!result.Success)
            {
                throw new UnsuccessfulTransactionException();
            }
        }
    }
}
