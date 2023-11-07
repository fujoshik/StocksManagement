using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        Task<decimal> CalculateCurrentYield(Guid id, decimal initialBalance, decimal currentBalance);
        bool IsValidMarketPrice(decimal currentBalance);
    }
}
