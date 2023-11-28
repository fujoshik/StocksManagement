using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/wallets")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        
        [HttpPost("deposit")]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        public async Task<ActionResult> DepositSum(DepositDto deposit)
        {
            await _walletService.DepositSumAsync(deposit);

            return Ok();
        }

        [HttpPost("{currency}")]
        [Authorize(Policy = PolicyConstants.AllowAll)]
        public async Task<ActionResult> ChangeCurrency([FromRoute] CurrencyCode currency)
        {
            await _walletService.ChangeCurrencyAsync(currency);

            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WalletResponseDto>> GetWalletInfoAsync(Guid id = default)
        {
            var wallet = await _walletService.GetWalletInfoAsync(id);

            return Ok(wallet);
        }
    }
}
