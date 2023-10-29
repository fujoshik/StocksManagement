using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Accounts.Domain.Providers
{
    public class UserDetailsProvider : IUserDetailsProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDetailsProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetAccountId()
        {
            Guid requesterId;

            var value = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (value is null || !Guid.TryParse(value.Value, out requesterId))
            {
                throw new IncorrectAccountIdException(nameof(value));
            }
            return requesterId;
        }
    }
}
