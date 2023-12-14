using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/stocks")]
    [Authorize(Policy = PolicyConstants.AllowAdminActiveAndTrialRoles)]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("buy-stock")]
        public async Task<IActionResult> BuyStock([FromQuery] BuyStockQuery buyStockQuery)
        {
            await _stockService.BuyStockAsync(buyStockQuery.Ticker, buyStockQuery.Quantity);

            return Ok();
        }

        [HttpPost("sell-stock")]
        public async Task<IActionResult> SellStock([FromQuery] BuyStockQuery sellStock)
        {
            await _stockService.SellStockAsync(sellStock.Ticker, sellStock.Quantity);

            return Ok();
        }
    }
}
