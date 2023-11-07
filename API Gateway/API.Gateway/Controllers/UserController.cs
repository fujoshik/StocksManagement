using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Gateway.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMemoryCache _cache;

        public UserController(ILogger<UserController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet("userData")]
        [Authorize] 
        public IActionResult GetUserData()
        {
            
            if (!_cache.TryGetValue(User.Identity.Name, out UserData userData))
            {
                
                userData = FetchUserData(User.Identity.Name); 
                _cache.Set(User.Identity.Name, userData, TimeSpan.FromHours(4));
            }

           
            _logger.LogInformation("Заявка за данни на потребител: {Username}", User.Identity.Name);

            return Ok(userData);
        }

        private UserData FetchUserData(string username)
        {
            var user = new UserData
            {
                Username = username,
                FirstName = "Maria",
                LastName = "Mara",
                Email = "maria.mara@gmail.com"
                
            };

            return user;
        }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
    }
}
