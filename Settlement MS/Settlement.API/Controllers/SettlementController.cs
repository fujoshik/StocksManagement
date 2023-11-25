using Microsoft.AspNetCore.Mvc;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.DTOs.Transaction;

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
                var response = await httpClientService.GetWalletBalance(walletId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStockByDateAndTicker(string date, string stockTicker)
        {
            try
            {
                var response = await httpClientService.GetStockByDateAndTicker(date, stockTicker);
                return Ok(response);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExecuteDeal(TransactionRequestDto transactionRequest)
        {
            try
            {
                var response = await settlementService.ExecuteDeal(transactionRequest);
                return Ok(response);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
