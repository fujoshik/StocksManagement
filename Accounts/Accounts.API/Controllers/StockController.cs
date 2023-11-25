using Accounts.API.Attributes;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/stocks")]
    [AuthorizeRoles(Role.Admin, Role.Regular, Role.Special, Role.Trial, Role.VIP)]
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
            //await _stockService.Sell(ticker, quantity);

            return Ok();
        }
    }
}
