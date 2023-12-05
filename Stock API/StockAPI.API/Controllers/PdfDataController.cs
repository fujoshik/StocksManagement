using Microsoft.AspNetCore.Mvc;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Infrastructure.Models.PdfData;

namespace StockAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfDataController : Controller
    {
        private readonly IPdfDataService _pdfDataService;
        public PdfDataController(IPdfDataService pdfDataService) 
        { 
            _pdfDataService = pdfDataService;
        }

        [HttpPost]
        [Route("add-pdf-message")]
        public async Task<IActionResult> GeneratePdf()
        {
            try
            {
                PdfData pdfData = new PdfData();
                pdfData.Title = "--Summary Of The Most Popular Stocks--";
                pdfData.Content = "no content";
                _pdfDataService.GeneratePdf(pdfData);
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
