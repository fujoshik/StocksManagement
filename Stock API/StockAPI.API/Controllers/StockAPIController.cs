using Microsoft.AspNetCore.Mvc;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Infrastructure.Models;

namespace StockAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockAPIController : Controller
    {
        private readonly IStockAPIService _stockAPIService;
        private readonly ILogger<StockAPIController> _logger;

        public StockAPIController(IStockAPIService stockAPIService, ILogger<StockAPIController> logger)
        {
            _stockAPIService = stockAPIService;
            _logger = logger;
        }

        [HttpGet]
        [Route("grouped-daily")]
        public async Task<IActionResult> GetGroupedDailyData()
        {
            try
            {
                var responseData = await _stockAPIService.GetGroupedDailyData();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var responseData = await _stockAPIService.GetGroupedDailyData();
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
