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
        private readonly ITransactionService _transactionService;

        public StockService(IUserDetailsProvider userDetailsProvider,
                            IAccountService accountService,
                            ISettlementService settlementService,
                            IWalletService walletService,
                            ITransactionService transactionService)
        {
            _userDetailsProvider = userDetailsProvider;
            _accountService = accountService;
            _settlementService = settlementService;
            _walletService = walletService;
            _transactionService = transactionService;
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

        public async Task SellStockAsync(string ticker, int quantity)
        {
            var currentAccount = await _accountService.GetByIdAsync(_userDetailsProvider.GetAccountId());

            var transactions = await _transactionService.GetSoldTransactionsByAccountAsync(currentAccount.Id);
            var tickerTransactions = transactions.Where(x => x.StockTicker == ticker).ToList();

            if (tickerTransactions.Count == 0)
            {
                throw new ArgumentException("You don't own stocks with this ticker!");
            }               
            if (tickerTransactions.Count < quantity)
            {
                throw new ArgumentException("You don't have enough stocks with this ticker!");
            }

            var result = await _settlementService.ExecuteDealAsync(new ExecuteDealDto
            {
                Ticker = ticker,
                Quantity = quantity,
                TransactionType = TransactionType.Sold,
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
