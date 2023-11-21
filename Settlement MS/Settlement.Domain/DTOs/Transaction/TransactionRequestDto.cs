using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.DTOs.Transaction
{
    public class TransactionRequestDto
    {
        public Guid WalletId { get; set; }
        public string Date { get; set; }
        public string StockTicker { get; set; }
        public int Quantity { get; set; }
        public Guid AccountId { get; set; }
    }
}
