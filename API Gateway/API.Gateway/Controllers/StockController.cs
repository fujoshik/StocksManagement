using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("buy-stock")]
        public async Task<IActionResult> BuyStock([FromQuery] BuyStockDTO buyStock)
        {
            await _stockService.BuyStockAsync(buyStock);

            return Ok();
        }

        [HttpPost("sell-stock")]
        public async Task<IActionResult> SellStock([FromQuery] string ticker, [FromQuery] int quantity)
        {
            //await _stockService.SellStockAsync(ticker, quantity);

            return Ok();
        }
    }
}
