using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.Abstracions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationControler : ControllerBase
    {
        private readonly ICalculationService calculationService;
        private readonly IHttpClientService httpClientAccaounts;

        public CalculationControler(ICalculationService calculationService)
        {
            this.calculationService = calculationService;
            this.httpClientAccaounts = httpClientAccaounts;
        }

        [HttpGet("calculate-current-yield")]
        public IActionResult CalculateCurrentYield(decimal amount, decimal currentBalance)
        {
            try
            {
                if (currentBalance <= 0)
                {
                    return BadRequest("Invalid current market price.");
                }

                decimal currentYield = calculationService.CalculateCurrentYield(amount, currentBalance);
                return Ok(new { CurrentYield = currentYield });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
