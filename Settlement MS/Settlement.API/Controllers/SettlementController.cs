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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountBalance(Guid id)
        {
            try
            {
                var response = await httpClientService.GetAccountBalance(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("{executeDeal}")]
        public async Task<IActionResult> ExecuteDeal(WalletResponseDto model, decimal price, int amount)
        {
            try
            {
                var response = await settlementService.ExecuteDeal(model, price, amount);
                return Ok(response);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
