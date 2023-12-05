using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/accounts")]  
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<ActionResult<AccountResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var account = await _accountService.GetByIdAsync(id);

            return Ok(account);
        }
    }
}
