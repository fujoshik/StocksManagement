using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Accounts.API.Policies.RolePolicy
{
    public class ValidRoleHandler : AuthorizationHandler<ValidRoleAuthorizationRequirement>
    {
        private readonly IDeleteWhenTrialEndsService _deleteWhenTrialEndsService;
        private readonly IUserDetailsProvider _userDetailsProvider;

        public ValidRoleHandler(IDeleteWhenTrialEndsService deleteWhenTrialEndsService,
                                IUserDetailsProvider userDetailsProvider)
        {
            _deleteWhenTrialEndsService = deleteWhenTrialEndsService;
            _userDetailsProvider = userDetailsProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, 
                                                             ValidRoleAuthorizationRequirement requirement)
        {
            var role = context
                .User
                .Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .FirstOrDefault().Value;

            var expirationDate = context
                .User
                .Claims
                .Where(x => x.Type == "DateToDelete")
                .FirstOrDefault().Value;

            var accountId = _userDetailsProvider.GetAccountId();

            if (requirement.Role.Any(r => r == role))
            {
                var result = await _deleteWhenTrialEndsService.DeleteAccountWhenTrialEndsAsync(accountId, expirationDate);

                if (result)
                {
                    context.Succeed(requirement);
                }                   
            }
            else
            {
                context.Fail();
            }           
        }
    }
}
