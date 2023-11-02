using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Accounts.Domain.DTOs.Wallet;

namespace Analyzer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzerControler : ControllerBase
    {
        private readonly IService iservice;
        public AnalyzerControler(IService iservice)
        {
            this.iservice = iservice;
        }


        [HttpGet("check-accounts")]
        public async Task<IActionResult> GetAccountInfo(Guid id)
        {
            WalletResponseDto accountData = await iservice.GetAccountInfoById(id);

            if (accountData != null)
            {
                return Ok(accountData);
            }

            return StatusCode(500, "Woopsie Daisy! Looks like something went completely wrong. You can try again later. ;)");
        }

        
    }
}
