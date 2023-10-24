using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Settlement.Domain.Abstraction.Services;

namespace Settlement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService settlementService;

        public SettlementController(ISettlementService settlementService)
        {
            this.settlementService = settlementService;
        }

        [HttpPost]
        [Route("checkAccount")]
        public async Task<IActionResult> CheckAccount(string userId, decimal amount)
        {
            try
            {
                var response = await settlementService.CheckAccount(userId, amount);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
