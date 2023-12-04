using Microsoft.AspNetCore.Mvc;
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
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("daily-weekly-monthly")]
        public async Task<IActionResult> FillData([FromQuery] DataOption dataOption, [FromQuery] string symbol)
        {
            try
            {
                var responseData = await _fillDatabaseService.FillData(dataOption, symbol);
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
