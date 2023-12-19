namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPercentageChange
    {
        Task<decimal> PercentageChange(Guid walletId, string stockTicker, string data);
    }
}
