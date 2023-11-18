using Analyzer.API.Analyzer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        Task<decimal> CalculateCurrentYieldForUser(Guid userId);
        decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks);
        Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
        Task<decimal> FetchPercentageChange(string stockTicker, string data);
        bool IsValidMarketPrice(decimal currentBalance);

    }
}