using Analyzer.API.Analyzer.Domain.DTOs;


namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        //Task<decimal> CalculateCurrentYield(Guid userId);

        //Task<List<TransactionResponseDto>> GetTransactionsAsync(Guid userId, string stockTicker);

        public Task<decimal> CalculateCurrentYield(Guid userId, string stockTicker, string data);
        public decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks);
        public Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
        public Task<decimal> PercentageChange(string stockTicker, string data);
        public bool IsValidMarketPrice(decimal currentBalance);

    }
}