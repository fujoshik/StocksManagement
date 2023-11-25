using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.Stock
{
    public class StockDataDTO
    {
        public string Symbol { get; set; }
        public decimal CurrentValue { get; set; }
        public List<decimal> HistoricalValues { get; set; }
        
    }

}
