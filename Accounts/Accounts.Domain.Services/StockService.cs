using Accounts.Domain.Abstraction.Factories;
using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Transaction;
using Accounts.Domain.Enums;
using IHttpClientFactory = Accounts.Domain.Abstraction.Factories.IHttpClientFactory;

namespace Accounts.Domain.Services
{
    public class StockService : IStockService
    {
        private readonly IUserDetailsProvider _userDetailsProvider;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IHttpClientFactory _httpClientFactory;

        public StockService(IUserDetailsProvider userDetailsProvider,
                            IAccountService accountService,
                            ITransactionService transactionService,
                            IHttpClientFactory httpClientFactory)
        {
            _userDetailsProvider = userDetailsProvider;
            _accountService = accountService;
            _transactionService = transactionService;
            _httpClientFactory = httpClientFactory;
        }

        //public async Task BuyStock(string ticker, int quantity)
        //{
        //    var currentAccount = await _accountService.GetByIdAsync(_userDetailsProvider.GetAccountId());

        //    var result = _httpClientFactory
        //        .GetSettlementClient()
        //        .BuyStock(ticker, quantity, currentAccount.WalletId, currentAccount.Role);

        //    var transactionRequest = new TransactionRequestDto()
        //    {
        //        AccountId = currentAccount.Id,
        //        Price = result.Price,
        //        Quantity = quantity,
        //        StockTicker = ticker,
        //        TransactionType = TransactionType.Bought,
        //    };

        //    await _transactionService.CreateAsync(transactionRequest);
        //}
    }
}
