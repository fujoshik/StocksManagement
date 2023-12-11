using Gateway.Domain.DTOs.Authentication;
using Gateway.Domain.DTOs.Stock;
using Gateway.Domain.DTOs.User;
using Gateway.Domain.DTOs.Wallet;
using Gateway.Domain.Enums;

namespace Gateway.Domain.Abstraction.Clients
{
    public interface IAccountClient
    {
        HttpClient GetApi();
        Task RegisterAsync(RegisterWithSumDTO registerDto);
        Task RegisterTrialAsync(RegisterTrialDTO registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task UpdateUser(Guid id, UserWithoutAccountIdDto user);
        Task DepositSumAsync(DepositSumDto deposit);
        Task ChangeCurrencyAsync(Currency currency);
        Task<WalletResponse> GetWalletInfoAsync(Guid id);
        Task BuyStockAsync(BuyStockDTO buyStock);
    }
}
