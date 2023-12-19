using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        public Task<decimal> CalculateCurrentYield(Guid accountId, string stockTicker, string data);
        public bool IsValidMarketPrice(decimal currentBalance);
        
    }
}