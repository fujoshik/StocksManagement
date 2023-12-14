using Microsoft.AspNetCore.Mvc;
using Accounts.Domain.DTOs.Wallet;
using StockAPI.Infrastructure.Models;
using Accounts.Domain.Abstraction.Services;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using Analyze.Domain.Service;
using static Analyzer.Domain.Abstracions.Interfaces.IService;

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
            WalletDto accountData = await httpClientService.GetAccountInfoById(id);

            if (accountData != null)
            {
                return Ok(accountData);
            }

            return StatusCode(500, "Woopsie Daisy! Looks like something went completely wrong. You can try again later. ;)");
        }

        [HttpGet("check-stockAPI/ticker")]
        public async Task<IActionResult> GetStockData(string stockTicker, string Data)
        {
            Stock stock = await httpClientService.GetStockData(stockTicker, Data);

            if (stock != null)
            {
                return Ok(stock);
            }

            return StatusCode(500, "Woopsie Daisy! Looks like something went completely wrong. You can try again later. ;)");
        }

        [HttpPost("get-transaction")]
        public async Task<IActionResult> ExecuteDealAsync(TransactionResponseDto transaction)
        {
            try
            {
                var transactions = await httpClientService.GetExecuteDeal(transaction);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving transactions: {ex.Message}");
            }
        }

        


    }
}