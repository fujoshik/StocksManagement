using Microsoft.AspNetCore.Mvc;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Infrastructure.Models;

namespace StockAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockAPIController : Controller
    {
        private readonly IStockAPIService _stockAPIService;

        public StockAPIController(IStockAPIService stockAPIService)
        {
            _stockAPIService = stockAPIService;
        }

        [HttpGet]
        [Route("grouped-daily")]
        public async Task<IActionResult> GetGroupedDailyData()
        {
            try
            {
                var responseData = await _stockAPIService.GetGroupedDailyData();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-database-stocks")]
        public async Task<IActionResult> GetAllStocks()
        {
            try
            {
                var responseData =  await _stockAPIService.GetAllStocks();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet]
        [Route("get-stock-by-date-and-ticker")]
        public async Task<IActionResult> GetStockByDateAndTicker([FromQuery] string date, [FromQuery] string stockTicker)
        {
            try
            {
                var stock = await _stockAPIService.GetStockByDateAndTickerAsync(date, stockTicker);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    Log.Information($"No stock found for date '{date}' and stock ticker '{stockTicker}'.");
                    return NotFound($"No stock found for date '{date}' and stock ticker '{stockTicker}'.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-stocks-by-date")]
        public async Task<IActionResult> GetStocksByDate([FromQuery] string date)
        {
            try
            {
                var stock = await _stockAPIService.GetStocksByDate(date);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    Log.Information($"No stock found for date '{date}'.");
                    return NotFound($"No stock found for date '{date}'.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("get-market-characteristics")]
        public async Task<IActionResult> GetStockMarketCharacteristics([FromQuery] string date)
        {
            try
            {
                var stock = await _stockAPIService.GetStockMarketCharacteristics(date);

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
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
