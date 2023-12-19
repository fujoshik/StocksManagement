using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/analysis")]   
    public class AnalyzerController : ControllerBase
    {
        private readonly IAnalyzerService _analyzerService;

        public AnalyzerController(IAnalyzerService analyzerService)
        {
            _analyzerService = analyzerService;
        }

        [HttpGet("average-income")]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        public async Task<ActionResult<decimal>> CalculateAverageIncome([FromQuery] string stockTicker, [FromQuery] string date)
        {
            var result = await _analyzerService.CalculateAverageIncomeForPeriodAsync(stockTicker, date);

            return Ok(result);
        }
    }
}
