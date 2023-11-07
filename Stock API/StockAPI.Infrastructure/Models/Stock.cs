using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models
{
    public class Stock
    {
        public string? StockTicker { get; set; }
        public decimal? ClosestPrice { get; set; }
        public decimal? HighestPrice { get; set; }
        public decimal? LowestPrice { get; set; }
        public int? TransactionCount { get; set; } // Number of transactions in the aggregate window.
        public double? OpenPrice { get; set; } // Open price for the symbol in the given time period.
        public bool? IsOTC { get; set; } // Indicates whether the stock is an OTC ticker.
        public long? UnixTimestamp { get; set; } // Unix Msec timestamp for the start of the aggregate window.
        public object? TradingVolume { get; set; } // Trading volume of the symbol in the given time period.
        public double? VolumeWeightedAveragePrice { get; set; } // Volume weighted average price.
        public string? Date { get; set; }
    }
}
