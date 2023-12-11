using Gateway.Domain.Abstraction.Factories;
using Gateway.Domain.Abstraction.Services;
using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;

namespace Gateway.Domain.Services
{
    public class WalletCustomService : IWalletCustomService
    {
        private readonly IHttpClientFactoryCustom _httpClientFactoryCustom;

        public WalletCustomService(IHttpClientFactoryCustom httpClientFactoryCustom)
        {
            _httpClientFactoryCustom = httpClientFactoryCustom;
        }

        public async Task DepositSumAsync(DepositSumDto deposit)
        {
            await _httpClientFactoryCustom.GetAccountClient().DepositSumAsync(deposit);
        }

        public async Task ChangeCurrencyAsync(Currency currency)
        {
            await _httpClientFactoryCustom.GetAccountClient().ChangeCurrencyAsync(currency);
        }

        public async Task<WalletResponse> GetWalletInfoAsync(Guid id)
        {
            return await _httpClientFactoryCustom.GetAccountClient().GetWalletInfoAsync(id);
        }
    }
}
