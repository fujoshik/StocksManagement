using Settlement.Domain.Abstraction.Services;

namespace Settlement.Domain.Services
{
    public class WalletRoutes : IWalletRoutes
    {
        public Dictionary<string, string> Routes { get; set; }
        public WalletRoutes()
        {
            Routes = new Dictionary<string, string>
            {
                { "GET", "https://localhost:7073/accounts-api/wallets/{id}" }
            };
        }
    }
}
