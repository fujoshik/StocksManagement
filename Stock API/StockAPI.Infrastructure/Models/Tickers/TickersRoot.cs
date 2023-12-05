using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models.Tickers
{
    public class TickersRoot
    {
        public List<TickersResult> results { get; set; }
    }
}
