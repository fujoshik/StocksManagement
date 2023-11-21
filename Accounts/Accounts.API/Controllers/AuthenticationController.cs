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
        [HttpPost("register-trial")]
        public IActionResult RegisterTrial(RegisterTrialDto registerTrial)
        {
            _service.Register(registerTrial);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterWithSumDto registerWithSumDto)
        {
            _service.Register(registerWithSumDto);

            return NoContent();
        }
    }
}
