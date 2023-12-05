using Accounts.Domain.DTOs.Wallet;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Caching.Memory;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Services
{
    public class WalletCacheService : IWalletCacheService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ISettlementRepository settlementRepository;

        public WalletCacheService(IMemoryCache memoryCache, ISettlementRepository settlementRepository)
        {
            this.memoryCache = memoryCache;
            this.settlementRepository = settlementRepository;
        }

        public async Task<WalletResponseDto> GetWalletFromCache(Guid walletId)
        {
            if (!memoryCache.TryGetValue($"Wallet{walletId}", out WalletResponseDto cachedWallet))
            {
                cachedWallet = await settlementRepository.GetWalletById(walletId);
                if (cachedWallet != null)
                {
                    memoryCache.Set($"Wallet_{walletId}", cachedWallet, TimeSpan.FromMinutes(10));
                }
            }

            return cachedWallet;
        }
        
        public async Task SetWalletInCache(Guid walletId, Guid accountId, WalletResponseDto wallet)
        {
            memoryCache.Set($"Wallet_{walletId}_{accountId}", wallet, TimeSpan.FromMinutes(10));
            await UpdateWalletInCache(walletId, accountId, wallet);
        }

        private async Task UpdateWalletInCache(Guid walletId, Guid accountId, WalletResponseDto updatedWalletData)
        {
            if(memoryCache.TryGetValue($"Wallet_{walletId}_{accountId}", out WalletResponseDto cachedWallet))
            {
                cachedWallet.Id = updatedWalletData.Id;
                cachedWallet.InitialBalance = updatedWalletData.InitialBalance;
                cachedWallet.CurrentBalance = updatedWalletData.CurrentBalance;
                cachedWallet.CurrencyCode = updatedWalletData.CurrencyCode;

                memoryCache.Set($"Wallet_{walletId}", cachedWallet, TimeSpan.FromMinutes(30));
            }
            else
            {
                memoryCache.Set($"Wallet_{walletId}", updatedWalletData, TimeSpan.FromMinutes(30));
            }
        }
    }
}
