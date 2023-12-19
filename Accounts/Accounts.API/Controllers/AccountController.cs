using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Accounts.Domain.DTOs.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/accounts")]
    [Authorize(Policy = PolicyConstants.AllowAll)]
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

        [HttpGet("logged-user")]
        public ActionResult<Guid> GetLoggedAccountId()
        {
            var accountId = _accountService.GetLoggedAccount();

            return Ok(accountId);
        }
    }
}
