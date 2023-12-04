using Gateway.Domain.Abstraction.Clients;
using Gateway.Domain.Abstraction.Settings;
using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.DTOs.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients
{
    public class AccountClient : IAccountClient
    {
        private HttpClient _httpClient;
        private readonly string _accountApiUrl;
        private readonly AccountSettings _accountSettings;
        private readonly UserSettings _userSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountClient(IOptions<HostSettings> hostSettings,
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

        public HttpClient GetApi()
        {
            return _httpClient;
        }

        public async Task RegisterAsync(RegisterWithSumDTO registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.RegisterRoute, registerDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }
        }

        public async Task RegisterTrialAsync(RegisterTrialDTO registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.RegisterTrialRoute, registerDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.LoginRoute, loginDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task UpdateUser(Guid id, UserWithoutAccountIdDto user)
        {
            string value = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"];
            _httpClient.DefaultRequestHeaders.Authorization
                         = new AuthenticationHeaderValue("Bearer", value.Replace("Bearer", ""));
            var updateRoute = _accountApiUrl + _userSettings.UpdateRoute + "/" + id;

            var response = await _httpClient.PutAsJsonAsync(updateRoute, user);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(response.StatusCode + response.ReasonPhrase);
            }
        }
    }
}