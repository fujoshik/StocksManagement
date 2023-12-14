namespace Accounts.Domain.Abstraction.Services
{
    public interface IAnalyzerService
    {
        Task<decimal> CalculateAverageIncomeAsync(string ticker);
    }
}
