using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.DTOs
{
    using Accounts.Domain.Enums;
    public class TransactionResponseDto
    {
        public Guid AccountId { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public TransactionType TransactionType { get; set; }

        
        public decimal CommissionFee { get; set; }
        public string TransactionId { get; set; }
    }
}
