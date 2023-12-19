using Gateway.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Gateway.Domain.Clients.AccountClient
{
    public abstract class BaseAccountClient
    {
        protected HttpClient _httpClient;
        protected readonly string _accountApiUrl;
        protected readonly AccountSettings _accountSettings;
        protected readonly UserSettings _userSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseAccountClient(IOptions<HostSettings> hostSettings,
                             IOptions<AccountSettings> accountSettings,
                             IOptions<UserSettings> userSettings,
                             IHttpContextAccessor httpContextAccessor)
        {
            _accountApiUrl = hostSettings.Value.Account;
            _accountSettings = accountSettings.Value;
            _userSettings = userSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_accountApiUrl)
            };
        }

        protected void AddAuthorizationHeader()
        {
            string value = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
            _httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", value.Replace("Bearer", ""));
        }
    }
}
