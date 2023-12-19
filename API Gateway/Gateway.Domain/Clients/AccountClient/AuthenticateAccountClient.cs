using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients.AccountClient
{
    public class AuthenticateAccountClient : BaseAccountClient, IAuthenticateAccountClient
    {
        public AuthenticateAccountClient(IOptions<HostSettings> hostSettings,
                                         IOptions<AccountSettings> accountSettings,
                                         IOptions<UserSettings> userSettings,
                                         IHttpContextAccessor httpContextAccessor)
            : base(hostSettings, accountSettings, userSettings, httpContextAccessor)
        {
        }

        public async Task RegisterAsync(RegisterWithSumDTO registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.RegisterRoute, registerDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task RegisterTrialAsync(RegisterTrialDTO registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.RegisterTrialRoute, registerDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.LoginRoute, loginDto);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task VerifyCodeAsync(string code)
        {
            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.VerifyCodeRoute, code);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }
    }
}
