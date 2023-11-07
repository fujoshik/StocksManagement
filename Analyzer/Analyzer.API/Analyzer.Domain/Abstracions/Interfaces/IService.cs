using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IService
    {
        Task<WalletResponseDto> GetAccountInfoById(Guid id);
      
        //Task<> GetInfoFromSettlement(string id);
    }
}
