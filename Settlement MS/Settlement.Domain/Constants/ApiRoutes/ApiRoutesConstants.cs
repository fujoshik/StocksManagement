using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Constants.ApiRoutes
{
    public class ApiRoutesConstants
    {
        public const string WalletGetRoute = "https://localhost:7073/accounts-api/wallets/{id}";
        public const string StockGetRoute = "https://localhost:7195/api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stockTicker}";
    }
}
