using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.Domain.DTOs
{
        using Accounts.Domain.Enums;  // Ако използвате TransactionType

        public class DailyYieldChangeDto
        {
            public string Date { get; set; }  // Дата на транзакцията
            public string StockTicker { get; set; }  // Символ на акцията
            public TransactionType TransactionType { get; set; }  // Тип на транзакцията (покупка или продажба)
            public int Quantity { get; set; }  // Брой закупени/продадени акции
            public decimal PurchasePrice { get; set; }  // Цена на закупената акция
            public decimal CurrentPrice { get; set; }  // Текуща цена на акцията
            public decimal DailyYield { get; set; }  // Промяна в дохода за деня (CurrentPrice - PurchasePrice)
        }
    
}
