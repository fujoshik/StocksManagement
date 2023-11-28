using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPortfolioRiskService
    {
        Task<decimal> CalculatePortfolioRisk(Guid accountId, string stockTicker, DateTime date);
    }
}
