using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Analyzer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/analysis")]
    [Authorize(Policy = PolicyConstants.AllowAll)]
    public class AnalyzerController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;

        public AnalyzerController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("average-income")]
        public async Task<ActionResult<CalculateCurrentYieldDto>> CalculateCurrentYield([FromQuery] string stockTicker, 
            [FromQuery] string date)
        {
            var result = await _analyzerService.CalculateCurrentYieldAsync(stockTicker, date);

            return Ok(result);
        }

        [HttpGet("percentage-change")]
        public async Task<ActionResult<PercentageChangeDto>> GetPercentageChange([FromQuery] string stockTicker, [FromQuery] string date)
        {
            var percentageChange = await _analyzerService.GetPercentageChangeAsync(stockTicker, date);

            return Ok(percentageChange);
        }

        [HttpGet("daily-yield-changes")]
        public async Task<ActionResult<List<DailyYieldChangeDto>>> GetDailyYieldChanges([FromQuery] string date,
            [FromQuery] string stockTicker)
        {
            var dailyYieldChanges = await _analyzerService.GetDailyYieldChangesAsync(date, stockTicker);

            return Ok(dailyYieldChanges);
        }
    }
}
