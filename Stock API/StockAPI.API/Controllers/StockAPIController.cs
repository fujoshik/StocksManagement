using Microsoft.AspNetCore.Mvc;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Domain.Services.Scheduling;
using StockAPI.Infrastructure.Models;
using System.Globalization;

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

        //get daily stocks from polygon
        [HttpGet]
        [Route("grouped-daily")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> GetGroupedDailyData()
        {
            try
            {
                var responseData = await _stockAPIService.GetGroupedDailyData();
                //how to add custom header
                //HttpContext.Response.Headers.Add("header", "value");
                if(!responseData.Any())
                {
                    return NoContent();
                }
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while fetching grouped daily " +
                                    "data and adding it to the database.");
                return StatusCode(500, "Internal server error");
            }
        }

        //get stock by date and ticker from polygon 
        [HttpGet]
        [Route("get-stock-by-date-and-ticker-from-api")]
        public async Task<IActionResult> GetStockByDateAndTickerFromAPI([FromQuery] string date, [FromQuery] string stockTicker)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest("please use yyyy-MM-dd format for inputting date.");
                }
                var stock = await _stockAPIService.GetStockByDateAndTickerFromAPI(date, stockTicker);

                if (stock!=null)
                {
                    return Ok(stock);
                }
                else
                {
                    Log.Information($"No stock found for date '{date}' and stock ticker '{stockTicker}'.");
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occurred while trying to retrieve and " +
                                    $"add {stockTicker} from {date}" +
                                    $" to the database.");
                return StatusCode(500, "Internal server error");
            }
        }

        //get all stocks from the database
        [HttpGet]
        [Route("get-database-stocks")]
        public async Task<IActionResult> GetAllStocks()
        {
            try
            {
                var responseData =  await _stockAPIService.GetAllStocks();
                if (!responseData.Any()) return NoContent();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while retrieving all stocks from the database.");
                return StatusCode(500, "Internal server error");
            }
        }


        //get stock by date and ticker from the database
        [HttpGet]
        [Route("get-stock-by-date-and-ticker")]
        public async Task<IActionResult> GetStockByDateAndTicker([FromQuery] string date, [FromQuery] string stockTicker)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd",
    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest("please use yyyy-MM-dd format for inputting date.");
                }
                var stock = await _stockAPIService.GetStockByDateAndTicker(date, stockTicker);

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
                Log.Error(ex, $"an error occured while trying to" +
                    $" retrieve data for the stock {stockTicker} from {date}.");
                return StatusCode(500, "Internal server error");
            }
        }

        //get all stocks by a certain date from the database
        [HttpGet]
        [Route("get-stocks-by-date")]
        public async Task<IActionResult> GetStocksByDate([FromQuery] string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd",
    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest("please use yyyy-MM-dd format for inputting date.");
                }
                var stocks = await _stockAPIService.GetStocksByDate(date);

                if (stocks != null && stocks.Any())
                {
                    return Ok(stocks);
                }
                else
                {
                    Log.Information($"No stocks found for date '{date}'.");
                    return NotFound($"No stocks found for date '{date}'.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to retrieve stocks from {date}.");
                return StatusCode(500, "Internal server error");
            }
        }

        //get characteristics of the stock market for a certain time period
        [HttpGet]
        [Route("get-market-characteristics")]
        public async Task<IActionResult> GetStockMarketCharacteristics([FromQuery] string date)
        {
            try
            {
                if (!DateTime.TryParseExact(date, "yyyy-MM-dd",
    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest("please use yyyy-MM-dd format for inputting date.");
                }
                var stock = await _stockAPIService.GetStockMarketCharacteristics(date);

                if (stock != null)
                {
                    return Ok(stock);
                }
                else
                {
                    return NotFound($"no stock data found for date '{date}'.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to retrieve market characteristics for {date}.");
                return StatusCode(500, "Internal server error");
            }
        }

        //daily event that periodically adds daily stocks to the database
        [HttpGet]
        [Route("invoke-event")]
        public Task<IActionResult> InvokeEvent()
        {
            DailyJob job = new DailyJob(_stockAPIService);
            try
            {
                job.Execute();
                return Task.FromResult<IActionResult>(Ok());
            }
            catch(Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return Task.FromResult<IActionResult>(StatusCode(500, "Internal server error"));
            }
        }
    }
}
