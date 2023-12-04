using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Infrastructure.Models;
using System;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService calculationService;
        private readonly IHttpClientService httpClientService;


        public CalculationController(ICalculationService calculationService, IHttpClientService httpClientService)
        {
            this.calculationService = calculationService;
            this.httpClientService = httpClientService;

        }

        //[HttpGet("calculate-current-yield1")]
        //public async Task<IActionResult> CalculateCurrentYield1([FromQuery] Guid userId)
        //{
        //    try
        //    {
        //        decimal currentYield = await calculationService.CalculateCurrentYieldForUser(userId);
        //        return Ok(new { CurrentYield = currentYield });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("calculate-current-yield")]
        public async Task<IActionResult> CalculateCurrentYield([FromQuery] Guid userId, string stockTicker, string data)
        {
            try
            {
                decimal currentYield = await calculationService.CalculateCurrentYield(userId, stockTicker, data);

                return Ok(new { CurrentYield = currentYield });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("percentage-change")]
        public async Task<IActionResult> PercentageChange([FromQuery] string stockTicker, [FromQuery] string data)
        {
            try
            {
                decimal percentageChange = await calculationService.PercentageChange(stockTicker, data);
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