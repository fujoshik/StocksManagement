using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        public Task<decimal> CalculateCurrentYield(Guid userId, string stockTicker, string data, TransactionResponseDto transaction);
        
        public Task<decimal> PercentageChange(Guid userId, string stockTicker, string data);
        public bool IsValidMarketPrice(decimal currentBalance);
        
    }
}