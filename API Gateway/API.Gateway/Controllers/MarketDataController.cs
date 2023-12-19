using Gateway.Domain.DTOs.HistoricalData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketDataController : ControllerBase
    {
        private readonly ILogger<MarketDataController> _logger;
        private readonly IMemoryCache _cache;

        public MarketDataController(ILogger<MarketDataController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("currentStockValue")]
        [AllowAnonymous]
        public IActionResult GetCurrentStockValue()
        {
            if (!_cache.TryGetValue("CurrentStockValue", out double currentStockValue))
            {

                currentStockValue = FetchCurrentStockValue();
                _cache.Set("CurrentStockValue", currentStockValue, TimeSpan.FromMinutes(15));
            }


            _logger.LogInformation("Request for current stock value.");

            return Ok(new { StockValue = currentStockValue });
        }

        [HttpGet("historicalStockData")]
        [AllowAnonymous]
        public IActionResult GetHistoricalStockData()
        {

            if (!_cache.TryGetValue("HistoricalStockData", out List<HistoricalDataDto> historicalData))
            {

                historicalData = FetchHistoricalStockData();
                _cache.Set("HistoricalStockData", historicalData, TimeSpan.FromHours(4));
            }


            _logger.LogInformation("Request for historical stock data.");

            return Ok(historicalData);
        }
      

        private double FetchCurrentStockValue()
        {

            return 100.0;
        }

        private List<HistoricalDataDto> FetchHistoricalStockData()
        {

            return new List<HistoricalDataDto>
        {
            new HistoricalDataDto { Date = DateTime.Now.AddYears(-1), Value = 90.0 },
            new HistoricalDataDto { Date = DateTime.Now.AddYears(-2), Value = 80.0 }

        };
        }
    }
}

