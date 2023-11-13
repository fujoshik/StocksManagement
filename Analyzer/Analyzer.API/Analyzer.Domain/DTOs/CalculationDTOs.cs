namespace Analyzer.API.Analyzer.Domain.DTOs
{
    public class CalculationDTOs
    {
        public string Symbol { get; set; }  
        public string Name { get; set; }  
        public decimal Quantity { get; set; } 
        public decimal CurrentPrice { get; set; } 
        public string Ticker { get; set; }
    }
}
