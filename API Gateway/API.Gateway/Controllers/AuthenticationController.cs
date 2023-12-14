using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Gateway.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAccountService _accountService;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous] 
        public async Task<ActionResult> Login(LoginDto account)
        {
            var token = await _accountService.LoginAsync(account);

            if (token is null)
            {
                _logger.LogWarning($"Login failed for user: {account.Email}");
                return Unauthorized();
            }

            _logger.LogInformation($"Successful user login: {account.Email}");
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterWithSumDTO account)
        {
            await _accountService.RegisterAsync(account);

            return NoContent();
        }

        [HttpPost("register-trial")]
        public async Task<IActionResult> RegisterTrial(RegisterTrialDTO account)
        {
            await _accountService.RegisterTrialAsync(account);

            return NoContent();
        }

        [HttpPost("verify")]
        public async Task<ActionResult> VerifyCode(string code)
        {
            await _accountService.VerifyCodeAsync(code);

            return NoContent();
        }
    }
}
