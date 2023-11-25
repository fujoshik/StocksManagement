using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    public class AnalyzerController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IStatisticsService _statisticsService;

        public AnalyzerController(IStockService stockService, IStatisticsService statisticsService)
        {
            _stockService = stockService ?? throw new ArgumentNullException(nameof(stockService));
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        [HttpGet("current-price/{symbol}")]
        public IActionResult GetCurrentStockPrice(string symbol)
        {
            try
            {
                var currentPrice = _stockService.GetCurrentPrice(symbol);
                return Ok(currentPrice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving current stock price: {ex.Message}");
            }
        }

        [HttpGet("historical-data/{symbol}")]
        public IActionResult GetHistoricalStockData(string symbol)
        {
            try
            {
                var historicalData = _stockService.GetHistoricalData(symbol);
                return Ok(historicalData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving historical stock data: {ex.Message}");
            }
        }

        [HttpGet("top-users")]
        public IActionResult GetTopUsers()
        {
            try
            {
                var topUsers = _statisticsService.GetTopUsersByRequests(10); 
                return Ok(topUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving top users: {ex.Message}");
            }
        }
        [HttpGet("analyze/{symbol}")]
        public IActionResult AnalyzeStockData(string symbol)
        {
            try
            {
                //decimal currentStockValue = _stockAPIService.GetStockValue(symbol);
                //string recommendation = currentStockValue < 50 ? "Buy" : "Sell";
                //return new StockAnalysisResult { Symbol = symbol, Recommendation = recommendation };
                var analysisResult = _stockService.AnalyzeStockData(symbol);
                return Ok(analysisResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error analyzing stock data: {ex.Message}");
            }
        }

        [HttpGet("request-statistics/{route}")]
        public IActionResult GetRequestStatistics(string route)
        {
            try
            {
                _statisticsService.LogRequest(route); 

                var requestCount = _statisticsService.GetRequestCount(route);
                return Ok(new { Route = route, RequestCount = requestCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving request statistics: {ex.Message}");
            }
        }


    }
}