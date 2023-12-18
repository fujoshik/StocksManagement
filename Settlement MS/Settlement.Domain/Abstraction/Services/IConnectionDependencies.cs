using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Routes;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IConnectionDependencies
    {
        public HttpClient Http { get; }
        public IWalletRoutes Wallet { get; }
        public IStockRoutes Stock { get; }
        public ISettlementRepository Repository { get; }
    }
}
