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

        public SettlementController(ISettlementService settlementService)
        {
            this.settlementService = settlementService;
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
