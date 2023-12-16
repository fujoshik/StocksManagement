using System;

namespace Analyzer.Domain.DTOs
{
    using Accounts.Domain.Enums;

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