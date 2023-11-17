using Analyzer.API.Analyzer.Domain.DTOs;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface IDailyYieldChanges
    {
        Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
    }

}
