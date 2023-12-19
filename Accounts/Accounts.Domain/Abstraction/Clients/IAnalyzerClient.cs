namespace Accounts.Domain.Abstraction.Clients
{
    public interface IAnalyzerClient
    {
        HttpClient GetAnalyzerClient();
        Task<decimal> CalculateAverageIncomeForPeriodAsync(Guid accountId, string ticker, string date);
    }
}
