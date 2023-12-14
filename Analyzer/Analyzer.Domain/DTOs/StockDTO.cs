using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.DTOs
{
    public class StockDTO
    {
        public DateTime Date { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
