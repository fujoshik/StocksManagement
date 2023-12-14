namespace Accounts.Domain.Abstraction.Clients
{
    public interface IAnalyzerClient
    {
        HttpClient GetAnalyzerClient();
        Task<decimal> CalculateAverageIncomeAsync(Guid accountId, string ticker);
    }
}
