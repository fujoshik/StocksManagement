using Microsoft.AspNetCore.Mvc;
using StockAPI.Infrastructure.Models;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;


namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerControler : ControllerBase
    {
        private readonly IHttpClientService httpClientService;
        public AnalyzerControler(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }


        [HttpGet("check-accounts")]
        public async Task<IActionResult> GetAccountInfo(Guid id)
        {
            try
            {
                WalletDto accountData = await httpClientService.GetAccountInfoById(id);

                if (accountData != null)
                {
                    return Ok(accountData);
                }

                return NotFound($"Account with ID '{id}' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");

                return BadRequest($"An error occurred while processing the request: {ex.Message}");
            }
        }


        [HttpGet("check-stockAPI/ticker")]
        public async Task<IActionResult> GetStockData(string stockTicker, string data)
        {
            try
            {
                Stock stock = await httpClientService.GetStockData(stockTicker, data);

                if (stock != null)
                {
                    return Ok(stock);
                }
                return NotFound($"Stock with ticker '{stockTicker}' and data '{data}' not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return BadRequest($"An error occurred while processing the request: {ex.Message}");
            }
        }


        [HttpPost("transactions")]
        public async Task<IActionResult> ExecuteDealAsync(TransactionResponseDto transaction)
        {
            try
            {
                var transactions = await httpClientService.GetExecuteDeal(transaction);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-transactions")]
        public async Task<ActionResult<List<TransactionResponseDto>>> GetTransactionsAsync([FromQuery] Guid accountId, [FromQuery] string stockTicker)
        {
            try
            {
                var transactions = await httpClientService.GetTransactions(accountId, stockTicker);
                return Ok(transactions);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}