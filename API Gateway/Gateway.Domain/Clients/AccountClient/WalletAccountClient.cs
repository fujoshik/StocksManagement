using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;
using Gateway.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients.AccountClient
{
    public class WalletAccountClient : BaseAccountClient, IWalletAccountClient
    {
        public WalletAccountClient(IOptions<HostSettings> hostSettings,
                                   IOptions<AccountSettings> accountSettings,
                                   IOptions<UserSettings> userSettings,
                                   IHttpContextAccessor httpContextAccessor) 
            : base(hostSettings, accountSettings, userSettings, httpContextAccessor)
        {
        }

        public async Task DepositSumAsync(DepositSumDto deposit)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.DepositRoute, deposit);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task ChangeCurrencyAsync(Currency currency)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PostAsJsonAsync(_accountApiUrl + _accountSettings.ChangeCurrencyRoute, currency);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task<WalletResponse> GetWalletInfoAsync(Guid id)
        {
            AddAuthorizationHeader();

            var response = await _httpClient.GetAsync(_accountApiUrl + _accountSettings.GetWalletInfoRoute);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<WalletResponse>();
        }
    }
}
