using Accounts.Domain.DTOs.Wallet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Services;

namespace Settlement.API.Controllers
{
    [ApiController]
    [Route("settlements-api")]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService settlementService;
        private readonly IHttpClientService httpClientService;

        public SettlementController(ISettlementService settlementService, IHttpClientService httpClientService)
        {
            this.settlementService = settlementService;
            this.httpClientService = httpClientService;
        }

        [HttpGet("{walletId}")]
        public async Task<IActionResult> GetWalletBalance(Guid walletId)
        {
            try
            {
                /*var apiName = HttpContext.Request.Headers["X-Api-Name"].FirstOrDefault();
                if (apiName != "Accounts.API")
                {
                    return BadRequest("Invalid API access.");
                }*/
                var response = await httpClientService.GetWalletBalance(walletId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /* testing...
        [HttpGet("{stockId}")]
        public async Task<IActionResult> GetStockPrice(string stockId)
        {
            try
            {
                var apiName = HttpContext.Request.Headers["X-Api-Name"].FirstOrDefault();
                if(apiName != "StockAPI.API")
                {
                   return BadRequest("Invalid API access.");
                }
                var response = await httpClientService.GetStockPrice(stockId);
                return Ok(response);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        */

        [HttpPost("{walletId}/{stockId}/{amount}")]
        public async Task<IActionResult> ExecuteDeal(Guid walletId, decimal price, int amount)
        {
            try
            {
                var response = await settlementService.ExecuteDeal(walletId, price, amount);
                return Ok(response);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
