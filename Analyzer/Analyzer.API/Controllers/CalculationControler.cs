
using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService calculationService;
        private readonly IHttpClientService httpClientAccounts;
        private readonly PercentageChangeCalculator percentageChangeCalculator;

        public CalculationController(ICalculationService calculationService, IHttpClientService httpClientAccounts, PercentageChangeCalculator percentageChangeCalculator)
        {
            this.calculationService = calculationService;
            this.httpClientAccounts = httpClientAccounts;
            this.percentageChangeCalculator = percentageChangeCalculator;
        }

        [HttpGet("calculate-current-yield")]
        public async Task<IActionResult> CalculateCurrentYield([FromQuery] Guid userId)
        {
            try
            {
                decimal currentYield = await calculationService.CalculateCurrentYieldForUser(userId);
                return Ok(new { CurrentYield = currentYield });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("calculate-percentage-change")]
        public async Task<IActionResult> CalculatePercentageChange(Guid userId, string stockTicker, string Data)
        {
            try
            {
                decimal percentageChange = await percentageChangeCalculator.CalculatePercentageChange(userId, stockTicker, Data);
                return Ok(new { PercentageChange = percentageChange });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating percentage change: {ex.Message}");
            }
        }

        [HttpGet("calculate-portfolio-risk")]
        public IActionResult CalculatePortfolioRisk([FromBody] List<CalculationDTOs> stocks)
        {
            decimal portfolioRisk = calculationService.CalculatePortfolioRisk(stocks);
            return Ok(new { PortfolioRisk = portfolioRisk });
        }

        [HttpGet("calculate-daily-yield-changes")]
        public async Task<IActionResult> CalculateDailyYieldChanges([FromBody] List<CalculationDTOs> stocks)
        {
            try
            {
                decimal dailyYieldChanges = await calculationService.CalculateDailyYieldChanges(stocks);
                return Ok(new { DailyYieldChanges = dailyYieldChanges });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error calculating daily yield changes: {ex.Message}");
            }
        }



    }
}
