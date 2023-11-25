using Settlement.Domain.Abstraction.Routes;
using Settlement.Domain.Constants.ApiRoutes;

namespace Settlement.Domain
{
    public class StockRoutes : IStockRoutes
    {
        public Dictionary<string, string> Routes { get; set; }

        public StockRoutes()
        {
            Routes = new Dictionary<string, string>
            {
                { "GET", ApiRoutesConstants.StockGetRoute }
            };
        }
    }
}
