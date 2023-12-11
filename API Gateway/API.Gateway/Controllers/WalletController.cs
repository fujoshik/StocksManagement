using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletCustomService _walletService;

        public WalletController(IWalletCustomService walletService)
        {
            _walletService = walletService;
        }

        [Authorize]
        [HttpPost("deposit")]
        public async Task<ActionResult> DepositSum(DepositSumDto deposit)
        {
            await _walletService.DepositSumAsync(deposit);

            return Ok();
        }

        [Authorize]
        [HttpPost("{currency}")]
        public async Task<ActionResult> ChangeCurrency([FromRoute] Currency currency)
        {
            await _walletService.ChangeCurrencyAsync(currency);

            return Ok();
        }

        [Authorize]
        [HttpGet("{id?}")]
        public async Task<ActionResult<WalletResponse>> GetWalletInfoAsync(Guid id = default)
        {
            var wallet = await _walletService.GetWalletInfoAsync(id);

            return Ok(wallet);
        }
    }
}
