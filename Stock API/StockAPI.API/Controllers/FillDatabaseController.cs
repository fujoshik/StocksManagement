using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Infrastructure.Enums;

namespace StockAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FillDatabaseController : Controller
    {
        private readonly IFillDatabaseService _fillDatabaseService;
        public FillDatabaseController(IFillDatabaseService fillDatabaseService)
        {
            _fillDatabaseService = fillDatabaseService;
        }

        //get a list of the tickers of all stocks available on polygon
        [HttpGet]
        [Route("tickers-list")]
        public async Task<IActionResult> GetTickersList()
        {
            try
            {
                var responseData = await _fillDatabaseService.GetTickersList();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while trying to retrieve all tickers from polygon.");
                return StatusCode(500, "Internal server error");
            }
        }

        //add daily, weekly or monthly stocks from alphavantage to the database
        [HttpPost]
        [Route("daily-weekly-monthly")]
        public async Task<IActionResult> FillData([FromQuery] DataOption dataOption, [FromQuery] string symbol)
        {
            try
            {
                var responseData = await _fillDatabaseService.FillData(dataOption, symbol);
                if (responseData.IsNullOrEmpty())
                {
                    return NotFound("no data for the specified ticker found.");
                }
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occurred while trying to retrieve {dataOption} " +
                    $"data for '{symbol}' from alphavantage.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
