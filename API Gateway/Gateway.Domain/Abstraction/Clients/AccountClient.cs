using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Clients
{
    public interface IAccountClient
    {
        HttpClient GetApi();
        Task RegisterAsync(RegisterWithSumDto registerDto);
        Task RegisterTrialAsync(RegisterTrialDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task UpdateUser(Guid id, UserWithoutAccountIdDto user);
    }
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

        public async Task RegisterAsync(RegisterWithSumDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.RegisterRoute, registerDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Unsuccessful request");
            }
        }

        public async Task RegisterTrialAsync(RegisterTrialDto registerDto)
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

