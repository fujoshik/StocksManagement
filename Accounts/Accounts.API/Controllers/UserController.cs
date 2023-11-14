using Accounts.API.Attributes;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.User;
using Accounts.Domain.Enums;
using Accounts.Domain.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.API.Controllers
{
    [ApiController]
    [Route("accounts-api/users")]
    [AuthorizeRoles(Role.Inactive, Role.Regular, Role.Special, Role.VIP)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<ActionResult<UserResponseDto>> GetByIdAsync([FromRoute] Guid id)
        {
            var user = await _userService.GetByIdAsync(id);

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<UserResponseDto>>> GetAllAsync(
            [FromQuery] PagingInfo pagingInfo)
        {
            var users = await _userService.GetPageAsync(pagingInfo);

            return Ok(users);
        }

        [HttpPut("{id}")]
        [AuthorizeRoles(Role.Inactive, Role.Regular, Role.Special, Role.VIP)]
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
    }
}
