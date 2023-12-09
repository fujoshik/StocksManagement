using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Constants
{
    public class APIsConection
    {
        public const string GetWallet = "https://localhost:7073/accounts-api/wallets/{id}";
        public const string GetStock = "https://localhost:7195/api/StockAPI/get-stock-by-date-and-ticker?date={date}&stockTicker={stockTicker}";
        public const string GetSettlementAPI = "https://localhost:7019/settlements-api";
        public const string GetTransactionsDetails = "https://localhost:7019/settlements-api";
    }
}

//$"/api/StockAPI/get-stock-by-date-and-ticker?date={Data}&stockTicker={stockTicker}"