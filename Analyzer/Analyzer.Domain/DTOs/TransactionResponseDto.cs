using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.DTOs
{
    using Accounts.Domain.Enums;
    using Analyzer.API.Analyzer.Domain.DTOs;

    public class TransactionResponseDto
    {
        public Guid WalletId { get; set; }
        public string Date { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public Guid AccountId { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
