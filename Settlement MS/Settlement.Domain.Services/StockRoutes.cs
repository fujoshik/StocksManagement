﻿using Settlement.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Services
{
    public class StockRoutes : IStockRoutes
    {
        public Dictionary<string, string> Routes { get; set; }

        public StockRoutes()
        {
            Routes = new Dictionary<string, string>
            {
                { "GET", "" }
            };
        }
    }
}
