using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models.GetGroupedDaily
{
    public class Result
    {
        public string? T { get; set; }
        public decimal? c { get; set; }
        public decimal? h { get; set; }
        public decimal? l { get; set; }
        public int? n { get; set; }
        public double? o { get; set; }
        public bool? otc { get; set; }
        public long? t { get; set; }
        public object? v { get; set; }
        public double? vw { get; set; }
    }
}
