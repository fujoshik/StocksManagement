using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Routes;
using Settlement.Domain.Abstraction.Services;

namespace Settlement.Domain.Dependencies
{
    public class ConnectionDependencies : IConnectionDependencies
    {
        public ConnectionDependencies(HttpClient httpClient, IWalletRoutes walletRoutes, IStockRoutes stockRoutes, ISettlementRepository settlementRepository)
        {
            Http = httpClient;
            Wallet = walletRoutes;
            Stock = stockRoutes;
            Repository = settlementRepository;
        }

        public HttpClient Http { get; }
        public IWalletRoutes Wallet { get; }
        public IStockRoutes Stock { get; }
        public ISettlementRepository Repository { get; }
    }
}
