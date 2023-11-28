using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.Trade
{
    public class TradeDTO
    {
        public string UserId { get; set; }
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public TradeType Type { get; set; }
        
    }

}
