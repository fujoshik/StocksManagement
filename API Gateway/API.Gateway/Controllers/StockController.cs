using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [Authorize]
        [HttpPost("buy-stock")]
        public async Task<IActionResult> BuyStock([FromQuery] BuyStockDTO buyStock)
        {
            await _stockService.BuyStockAsync(buyStock);

            return Ok();
        }

        [Authorize]
        [HttpPost("sell-stock")]
        public async Task<IActionResult> SellStock([FromQuery] BuyStockDTO sellStock)
        {
            //await _stockService.SellStockAsync(ticker, quantity);

            return Ok();
        }

        [HttpGet("grouped-daily")]
        public async Task<ActionResult<List<StockDTO>>> GetGroupedDailyData()
        {
            try
            {
                var responseData = await _stockService.GetGroupedDailyData();

                return Ok(responseData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-stock-by-date-and-ticker-from-api")]
        public async Task<ActionResult<StockDTO>> GetStockByDateAndTickerFromAPI([FromQuery] string date, [FromQuery] string stockTicker)
        {
            try
            {
                var stock = await _stockService.GetStockByDateAndTickerFromAPI(date, stockTicker);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    return NotFound($"No stock found for date '{date}' and stock ticker '{stockTicker}'.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-stock-by-date-and-ticker")]
        public async Task<ActionResult<StockDTO>> GetStockByDateAndTicker([FromQuery] string date, [FromQuery] string stockTicker)
        {
            try
            {
                var stock = await _stockService.GetStockByDateAndTicker(date, stockTicker);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    return NotFound($"No stock found for date '{date}' and stock ticker '{stockTicker}'.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-stocks-by-date")]
        public async Task<ActionResult<List<StockDTO>>> GetStocksByDate([FromQuery] string date)
        {
            try
            {
                var stocks = await _stockService.GetStocksByDate(date);

                if (stocks != null && stocks.Any())
                {
                    return Ok(stocks);
                }
                else
                {
                    return NotFound($"No stocks found for date '{date}'.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-market-characteristics")]
        public async Task<ActionResult<StockMarketCharacteristicsDTO>> GetStockMarketCharacteristics([FromQuery] string date)
        {
            try
            {
                var stock = await _stockService.GetStockMarketCharacteristics(date);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    return NotFound($"No stock data found for date '{date}'.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
