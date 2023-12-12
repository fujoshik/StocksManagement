using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using StockAPI.Infrastructure.Models;
using System;
using Analyze.Domain.Service;
using Analyzer.API.Analyzer.Domain.Abstracions.Services;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService calculationService;
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges yieldService;


        public CalculationController(ICalculationService calculationService, IHttpClientService httpClientService, IDailyYieldChanges yieldService)
        {
            this.calculationService = calculationService;
            this.httpClientService = httpClientService;
            this.yieldService = yieldService;

        }



        [HttpGet("calculate-current-yield")]
        public async Task<IActionResult> CalculateCurrentYield(Guid userId, string stockTicker, string data, TransactionResponseDto transaction)
        {
            try
            {
                decimal currentYield = await calculationService.CalculateCurrentYield(userId, stockTicker, data, transaction);

                return Ok(currentYield);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating current yield: {ex.Message}");
            }
        }

        [HttpGet("percentage-change")]
        public async Task<IActionResult> PercentageChange([FromQuery] Guid userId, [FromQuery] string stockTicker, [FromQuery] string data)
        {
            try
            {
                decimal percentageChange = await calculationService.PercentageChange(userId, stockTicker, data);
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


        [HttpGet("calculate-daily-yield-changes")]
        public IActionResult CalculateDailyYieldChanges([FromBody] List<CalculationDTOs> stockData)
        {
            try
            {
                if (stockData == null || stockData.Count < 2)
                {
                    return BadRequest("Insufficient data for calculating daily yield changes.");
                }

                List<decimal> result = yieldService.CalculateDailyYieldChanges(stockData);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating daily yield changes: {ex.Message}");
            }
        }



    }
}