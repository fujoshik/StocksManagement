using Accounts.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Accounts.API.Policies.RolePolicy
{
    public class ValidRoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public string[] Role { get; }

        public ValidRoleAuthorizationRequirement(params string[] role)
        {
            Role = role;
        }
    }
}
