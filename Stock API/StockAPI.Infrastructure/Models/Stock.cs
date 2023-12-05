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
        public int? TransactionCount { get; set; } 
        public double? OpenPrice { get; set; } 
        public bool? IsOTC { get; set; } 
        public long? UnixTimestamp { get; set; } 
        public object? TradingVolume { get; set; } 
        public double? VolumeWeightedAveragePrice { get; set; } 
        public string? Date { get; set; }
    }
}
