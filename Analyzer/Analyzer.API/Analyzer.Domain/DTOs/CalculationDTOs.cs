namespace Analyzer.API.Analyzer.Domain.DTOs
{
    public class CalculationDTOs
    {
        public decimal? HighestPrice { get; set; }
        public decimal? LowestPrice { get; set; }
        public string Ticker { get; set; }
        public string Date { get; set; } 
    }
}