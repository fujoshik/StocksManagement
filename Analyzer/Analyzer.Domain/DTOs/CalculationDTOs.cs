namespace Analyzer.API.Analyzer.Domain.DTOs
{
    using System.Collections.Generic;
    using Accounts.Domain.Enums;
    using Accounts.Domain.DTOs.Transaction;
    public class CalculationDTOs
    {
        public decimal? HighestPrice { get; set; }
        public decimal? LowestPrice { get; set; }
        public string Ticker { get; set; }
        public string Date { get; set; }

        public TransactionType TransactionType { get; set; }
        public List<TransactionResponseDto> DividendTransactions { get; set; }
    }
}