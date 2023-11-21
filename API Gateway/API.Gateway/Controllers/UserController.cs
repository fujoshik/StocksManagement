﻿using Gateway.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace API.Gateway.Controllers
{
    [Route("api/users")]
    [ApiController]
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

         [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UserWithoutAccountIdDto user)
        {
            object value = await _userService.UpdateAsync(id, user);

            return NoContent();
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

           
            _logger.LogInformation("Request for user data: {Username}", User.Identity.Name);

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
