using Settlement.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Wallet;
using Newtonsoft.Json;

namespace Settlement.Domain.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly IWalletRoutes walletRoutes;

        public HttpClientService(HttpClient httpClient, IWalletRoutes walletRoutes)
        {
            this.httpClient = httpClient;
            this.walletRoutes = walletRoutes;
        }

        public async Task<WalletResponseDto> GetAccountBalance(Guid id)
        {
            HttpResponseMessage response = await httpClient.GetAsync(walletRoutes.Routes["GET"].Replace("{id}", id.ToString()));
            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                WalletResponseDto wallet = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                return await Task.FromResult(wallet);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
