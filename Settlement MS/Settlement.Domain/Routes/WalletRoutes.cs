using Settlement.Domain.Abstraction.Routes;
using Settlement.Domain.Constants.ApiRoutes;

namespace Settlement.Domain
{
    public class WalletRoutes : IWalletRoutes
    {
        public Dictionary<string, string> Routes { get; set; }

        public WalletRoutes()
        {
            Routes = new Dictionary<string, string>
            {
                { "GET", ApiRoutesConstants.WalletGetRoute }
            };
        }
    }
}
