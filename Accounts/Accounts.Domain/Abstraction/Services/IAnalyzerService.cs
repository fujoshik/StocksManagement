using Accounts.Domain.DTOs.Analyzer;

namespace Accounts.Domain.Abstraction.Services
{
    public interface IAnalyzerService
    {
        Task<CalculateCurrentYieldDto> CalculateCurrentYieldAsync(string ticker, string date);
        Task<PercentageChangeDto> GetPercentageChangeAsync(string ticker, string date);
        Task<List<DailyYieldChangeDto>> GetDailyYieldChangesAsync(string date, string ticker);
    }
}
