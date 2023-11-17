using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.API.Analyzer.Domain;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        Task<decimal> CalculateCurrentYieldForUser(Guid userId);
        decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks);
        Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
        Task<decimal> FetchPercentageChange(string stockTicker, string data);
        bool IsValidMarketPrice(decimal currentBalance);

    }
}