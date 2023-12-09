using Analyzer.API.Analyzer.Domain.DTOs;


namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        public Task<decimal> CalculateCurrentYield(Guid userId, string stockTicker, string data);
        public Task<decimal> PercentageChange(Guid userId, string stockTicker, string data);
        public bool IsValidMarketPrice(decimal currentBalance);

    }
}