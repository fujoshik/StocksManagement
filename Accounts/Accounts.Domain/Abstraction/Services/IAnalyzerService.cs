namespace Accounts.Domain.Abstraction.Services
{
    public interface IAnalyzerService
    {
        Task<decimal> CalculateAverageIncomeForPeriodAsync(string ticker, string date);
    }
}
