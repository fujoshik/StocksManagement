using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace API.Gateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ILogger<AuthenticationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous] 
        public IActionResult Login([FromBody] LoginModel model)
        {
           
            if (IsUserAuthenticated(model.Username, model.Password))
            {
                _logger.LogInformation("Успешен вход за потребител: {Username}", model.Username);
                return Ok();
            }
            else
            {
                _logger.LogWarning("Неуспешен вход за потребител: {Username}", model.Username);
                return Unauthorized("Грешно потребителско име или парола");
            }
        }

        [HttpPost("logout")]
        [Authorize] 
        public IActionResult Logout()
        {
            
            _logger.LogInformation("Изход от системата за потребител: {Username}", User.Identity.Name);

            

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
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
