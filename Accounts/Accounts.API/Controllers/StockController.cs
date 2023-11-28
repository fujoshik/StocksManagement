using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
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
        public async Task<IActionResult> BuyStock([FromQuery] string ticker, [FromQuery] int quantity)
        {
            await _stockService.BuyStockAsync(ticker, quantity);

            return Ok();
        }

        [HttpPost("sell-stock")]
        public async Task<IActionResult> SellStock([FromQuery] string ticker, [FromQuery] int quantity)
        {
            await _stockService.SellStockAsync(ticker, quantity);

            return Ok();
        }
    }
}
