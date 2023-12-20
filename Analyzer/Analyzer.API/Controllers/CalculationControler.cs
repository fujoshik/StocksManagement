using Analyzer.API.Analyzer.Domain;
using Analyzer.Domain.Abstracions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService calculationService;
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges yieldService;
        private readonly IPercentageChange pesantageChanges;
       


        public CalculationController(ICalculationService calculationService, IHttpClientService httpClientService, IDailyYieldChanges yieldService, IPercentageChange pesantageChanges)
        {
            this.calculationService = calculationService;
            this.httpClientService = httpClientService;
            this.yieldService = yieldService;
            this.pesantageChanges = pesantageChanges;

        }


        [HttpGet("calculate-current-yield")]
        public async Task<IActionResult> CalculateCurrentYield(Guid accountId, string stockTicker, string data)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;

                decimal currentYield = await calculationService.CalculateCurrentYield(accountId, stockTicker, data);

                return Ok(new { CurrentYield = currentYield, CurrentDateTime = currentDateTime });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculate current yield: {ex.Message}");
            }
        }


        [HttpGet("percentage-change")]
        public async Task<IActionResult> PercentageChange([FromQuery] Guid walletId, [FromQuery] string stockTicker, [FromQuery] string data)
        {
            try
            {
                decimal percentageChange = await pesantageChanges.PercentageChange(walletId, stockTicker, data);
                return Ok(new { PercentageChange = percentageChange });
            }
            catch (UserDataNotFoundException ex)
            {
                return NotFound($"User data not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating percentage change: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("daily-yield-changes")]
        public async Task<IActionResult> DailyYieldChanges([FromQuery] string date, [FromQuery] string stockTicker, [FromQuery] Guid accountId)
        {
            try
            {
                DateTime startDate = DateTime.Parse(date);

                DateTime endDate = startDate.AddDays(4); 
                var stockList = await httpClientService.GetStock(stockTicker, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

                if (stockList != null && stockList.Any())
                {
                    var dailyYieldChanges = await yieldService.CalculateDailyYieldChanges(accountId, stockTicker, startDate, endDate, stockList);
                    return Ok(dailyYieldChanges);
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


    }
}