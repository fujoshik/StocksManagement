using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/transactions")]   
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("get-transactions")]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetTransactionsAsync([FromQuery] Guid accountId, 
            [FromQuery] string stockTicker)
        {
            return Ok(await _transactionService.GetTransactionsByAccountIdAndTickerAsync(accountId, stockTicker));
        }
    }
}
