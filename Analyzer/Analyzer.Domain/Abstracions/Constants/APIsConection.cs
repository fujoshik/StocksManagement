﻿using System;
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
    }
}

//$"/api/StockAPI/get-stock-by-date-and-ticker?date={Data}&stockTicker={stockTicker}"