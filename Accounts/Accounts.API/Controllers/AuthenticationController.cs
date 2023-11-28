using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/authentication")]    
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _service;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _service = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDto account)
        {
            var token = await _service.LoginAsync(account);

            if (token is null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("check-token")]
        public IActionResult CheckIfTokenIsValid(string token)
        {
            if (!_service.ValidateToken(token))
            {
                return Unauthorized();
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register-trial")]
        public async Task<ActionResult> RegisterTrialAsync(RegisterTrialDto registerTrial)
        {
            await _service.SendVerificationEmailAsync(registerTrial);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterWithSumDto registerWithSumDto)
        {
            await _service.SendVerificationEmailAsync(registerWithSumDto);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("verify")]
        public async Task<ActionResult> VerifyCode(string code)
        {
            if (!await _service.VerifyCodeAsync(code))
            {
                return Unauthorized();
            }

            return NoContent();
        }
    }
}
