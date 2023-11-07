using StockAPI.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models
{
    public class StockMarketCharacteristics
    {
        public MarketTrend MarketTrend { get; set; }
        public decimal? PercentageDifference { get; set; }
    }
}
