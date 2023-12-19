using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.User;
using Gateway.Domain.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Gateway.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMemoryCache _cache;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IMemoryCache cache, IUserService userService)
        {
            _logger = logger;
            _cache = cache;
            _userService = userService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<ActionResult<UserResponseDTO>> GetByIdAsync([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<UserResponseDTO>>> GetPageAsync(
            [FromQuery] Paging pagingInfo)
        {
            var users = await _userService.GetPageAsync(pagingInfo);

            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UserWithoutAccountIdDto user)
        {
            await _userService.UpdateAsync(id, user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _userService.DeleteAsync(id);

            return NoContent();
        }

        //[HttpGet("userData")]
        //[Authorize] 
        //public IActionResult GetUserData()
        //{
            
        //    if (!_cache.TryGetValue(User.Identity.Name, out UserData userData))
        //    {
                
        //        userData = FetchUserData(User.Identity.Name); 
        //        _cache.Set(User.Identity.Name, userData, TimeSpan.FromHours(4));
        //    }

           
        //    _logger.LogInformation("Request for user data: {Username}", User.Identity.Name);

        //    return Ok(userData);
        //}
       
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
}
