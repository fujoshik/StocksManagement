using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models
{
    public class StockByDateAndTickerRoot
    {
        public string status { get; set; }
        public string from { get; set; }
        public string symbol { get; set; }
        public double open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public object? volume { get; set; }
        public double afterHours { get; set; }
        public double preMarket { get; set; }
    }
}
