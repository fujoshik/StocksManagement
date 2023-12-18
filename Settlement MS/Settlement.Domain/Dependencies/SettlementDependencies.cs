using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Services;

namespace Settlement.Domain.Dependencies
{
    public class SettlementDependencies : ISettlementDependencies
    {
        public SettlementDependencies(IHttpClientService http, ISettlementRepository repository, IWalletCacheService cache)
        {
            Http = http;
            Repository = repository;
            Cache = cache;
        }

        public IHttpClientService Http { get; }
        public ISettlementRepository Repository { get; }
        public IWalletCacheService Cache { get; }
    }
}
