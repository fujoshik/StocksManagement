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
        private readonly IPercentageChange pesantageChanges;


        public CalculationController(ICalculationService calculationService, IHttpClientService httpClientService, IDailyYieldChanges yieldService, IPercentageChange pesantageChanges)
        {
            this.calculationService = calculationService;
            this.httpClientService = httpClientService;
            this.yieldService = yieldService;
            this.pesantageChanges = pesantageChanges;

        }



        [HttpGet("calculate-current-yield")]
        public async Task<IActionResult> CalculateCurrentYield(Guid userId, string stockTicker, string data)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;

                TransactionResponseDto currentYield = await calculationService.CalculateCurrentYield(userId, stockTicker, data);

                return Ok(new { CurrentYield = currentYield, CurrentDateTime = currentDateTime });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"InvalidOperationException: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, $"Error calculating current yield: {ex.Message}");
            }
        }


        [HttpGet("percentage-change")]
        public async Task<IActionResult> PercentageChange([FromQuery] Guid userId, [FromQuery] string stockTicker, [FromQuery] string data)
        {
            try
            {
                decimal percentageChange = await pesantageChanges.PercentageChange(userId, stockTicker, data);
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
        public async Task<IActionResult> CalculateDailyYieldChanges(Guid accountId, string stockTicker)
        {
            try
            {
                var result = await yieldService.DailyYieldChanges(accountId, stockTicker);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calculating daily yield changes: {ex.Message}");
            }
        }



    }
}