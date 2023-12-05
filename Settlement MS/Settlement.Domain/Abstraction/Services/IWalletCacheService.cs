using Accounts.Domain.DTOs.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IWalletCacheService
    {
        Task<WalletResponseDto> GetWalletFromCache(Guid walletId);
        Task SetWalletInCache(Guid walletId, Guid accountId, WalletResponseDto wallet);
    }
}
