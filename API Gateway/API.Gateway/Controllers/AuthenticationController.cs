using Gateway.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

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
        public async Task<ActionResult<string>> Login(LoginDto account)
        {
            var token = await _accountService.LoginAsync(account);

            if (token is null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }
        public IActionResult Login([FromBody] LoginModel model)
        {
           
            if (IsUserAuthenticated(model.Username, model.Password))
            {
                _logger.LogInformation("Successful user login: {Username}", model.Username);
                return Ok();
            }
            else
            {
                _logger.LogWarning("Login failed for user: {Username}", model.Username);
                return Unauthorized("Wrong username or password");
            }
        }

        [HttpPost("logout")]
        [Authorize] 
        public IActionResult Logout()
        {
            
            _logger.LogInformation("User logout: {Username}", User.Identity.Name);

            

            return Ok();
        }

        private bool IsUserAuthenticated(string username, string password)
        {
            
            if (username == "user" && password == "password")
            {
                return true;
            }
            return false;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterWithSumDto account)
        {
            await _accountService.RegisterAsync(account);

            return NoContent();
        }

        [HttpPost("register-trial")]
        public async Task<IActionResult> RegisterTrial(RegisterTrialDto account)
        {
            await _accountService.RegisterTrialAsync(account);

            return NoContent();
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
