﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services;
using StockAPI.Infrastructure.Models.PdfData;
using System.Globalization;

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

        //get pdf file with info about the most sold stocks in a given period
        [HttpGet]
        [Route("add-pdf-message")]
        public async Task<IActionResult> GeneratePdf([FromQuery] string beginningDate, [FromQuery] string endDate)
        {
            try
            {
                if (!DateTime.TryParseExact(beginningDate, "yyyy-MM-dd", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _)|| 
                    !DateTime.TryParseExact(endDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    return BadRequest("please use yyyy-MM-dd format for inputting date.");
                }

                await _pdfDataService.GeneratePdf(beginningDate, endDate);
                return Created("PdfData/","pdf file generated successfully.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
