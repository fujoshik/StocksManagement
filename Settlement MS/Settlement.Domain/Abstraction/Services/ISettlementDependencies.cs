using Settlement.Domain.Abstraction.Repository;

namespace Settlement.Domain.Abstraction.Services
{
    public interface ISettlementDependencies
    {
        public IHttpClientService Http { get; }
        public ISettlementRepository Repository { get; }
        public IWalletCacheService Cache { get; }
    }
}
