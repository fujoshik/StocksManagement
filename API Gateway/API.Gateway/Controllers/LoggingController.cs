using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    public class LoggingController : Controller
    {
        private readonly ILogger<LoggingController> _logger;

        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpGet("logData")]
        public IActionResult GetLogData()
        {

            string currentDate = DateTime.UtcNow.ToString("yyyyMMdd");
            string logFilePath = Path.Combine("Logs", currentDate + ".log");

            if (System.IO.File.Exists(logFilePath))
            {
                string logData = System.IO.File.ReadAllText(logFilePath);
                return Ok(logData);
            }
            else
            {
                return NotFound("Логовете за текущия ден не са налични.");
            }
        }
    }
}