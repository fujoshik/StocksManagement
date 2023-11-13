using Accounts.Domain.DTOs.Wallet;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IHttpClientService
    {
        Task<WalletResponseDto> GetUserDataById(string endpoint, Guid userId);
        HttpClient GetAccountClient();
        HttpClient GetStockAPI();
    }
}
