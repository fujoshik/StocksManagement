using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;  

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
        public async Task<IActionResult> CalculateCurrentYield(Guid id, decimal initialBalance, decimal currentBalance)
        {
            try
            {
                if (!calculationService.IsValidMarketPrice(currentBalance))
                {
                    return BadRequest("Invalid current market price.");
                }

                decimal currentYield = await calculationService.CalculateCurrentYield(id, initialBalance, currentBalance);
                return Ok(new { CurrentYield = currentYield });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
