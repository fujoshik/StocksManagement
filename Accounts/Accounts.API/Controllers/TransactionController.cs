using Accounts.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [Route("accounts-api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("get-transactions")]
        public async Task GetTransactionsAsync(Guid accountId, string stockTicker, DateTime dateTime)
        {
            await _transactionService.GetTransactionsByAccountIdTickerAndDateAsync(accountId, stockTicker, dateTime);
        }
    }
}
