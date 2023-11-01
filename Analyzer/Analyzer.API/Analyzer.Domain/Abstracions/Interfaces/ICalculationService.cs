using Analyzer.API.Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        public decimal CalculateCurrentYield(decimal amount, decimal currentBalance);
        bool IsValidMarketPrice(decimal currentBalance);
    }
}
