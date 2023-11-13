using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        Task<decimal> CalculateCurrentYieldForUser(Guid userId);
        public Task<decimal> CalculatePercentageChange(Guid userId, string stockTicker);
        decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks);
        public Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
        bool IsValidMarketPrice(decimal currentBalance);
    }
}