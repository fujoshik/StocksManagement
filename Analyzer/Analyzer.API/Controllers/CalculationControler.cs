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


        public CalculationControler(ICalculationService calculationService, IHttpClientService httpClientAccaounts)
        {
            this.calculationService = calculationService;
            this.httpClientAccaounts = httpClientAccaounts;
        }

        [HttpGet("calculate-current-yield")]
        public IActionResult CalculateCurrentYield([FromBody] WalletResponseDto walletResponseDto)
        {
            try
            {
                decimal currentYield = calculationService.CalculateCurrentYield(walletResponseDto);

                return Ok(new { CurrentYield = currentYield });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
