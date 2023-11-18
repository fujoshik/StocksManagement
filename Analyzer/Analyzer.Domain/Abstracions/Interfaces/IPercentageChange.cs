namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPercentageChange
    {
        Task<decimal> FetchPercentageChange(string stockTicker, string data);
    }
}
