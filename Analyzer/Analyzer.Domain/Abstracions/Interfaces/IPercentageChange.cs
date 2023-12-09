namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPercentageChange
    {
        Task<decimal> PercentageChange(Guid userId, string stockTicker, string data);
    }
}
