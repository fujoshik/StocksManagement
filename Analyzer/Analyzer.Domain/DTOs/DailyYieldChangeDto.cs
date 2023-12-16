using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.Domain.DTOs
{
        using Accounts.Domain.Enums;  

        public class DailyYieldChangeDto
        {
            public string Date { get; set; } 
            public string StockTicker { get; set; }  
            public TransactionType TransactionType { get; set; }  
            public int Quantity { get; set; } 
            public decimal PurchasePrice { get; set; }  
            public decimal CurrentPrice { get; set; } 
            public decimal DailyYield { get; set; } 
        }
    
}
