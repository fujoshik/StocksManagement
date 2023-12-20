using Accounts.Domain.DTOs.Analyzer;

namespace Accounts.Domain.Abstraction.Clients
{
    public interface IAnalyzerClient
    {
        HttpClient GetAnalyzerClient();
        Task<PercentageChangeDto> GetPercentageChangeAsync(Guid walletId, string ticker, string date);
        Task<CalculateCurrentYieldDto> CalculateAverageIncomeForPeriodAsync(Guid accountId, string ticker, string date);
        Task<List<DailyYieldChangeDto>> GetDailyYieldChangesAsync(string date, string ticker, Guid accountId);
    }
}
