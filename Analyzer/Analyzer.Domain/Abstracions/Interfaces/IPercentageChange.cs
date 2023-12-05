namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPercentageChange
    {
        Task<decimal> PercentageChange(string stockTicker, string data);
    }
}
