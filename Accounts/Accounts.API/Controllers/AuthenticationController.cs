using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
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

        //[AllowAnonymous]
        //[HttpPost("login")]
        //public IActionResult Login(LoginDto account)
        //{
        //    var token = _service.Login(account);

        //    if (token is null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(token);
        //}

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterDto account)
        {
            _service.Register(account);

            return NoContent();
        }
    }
}
