namespace Analyzer.API.Analyzer.Domain.DTOs
{
    using System.Collections.Generic;
    using Accounts.Domain.Enums;
    using Accounts.Domain.DTOs.Transaction;
    public class CalculationDTOs
    {   
        public DateTime Date { get; set; }   
        public decimal StockPrice { get; set; }  
    }
}