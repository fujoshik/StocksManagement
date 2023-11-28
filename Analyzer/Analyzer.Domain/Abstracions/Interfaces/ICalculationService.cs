using Accounts.Domain.DTOs.Transaction;
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
        //Task<decimal> CalculateCurrentYield(Guid userId);

        //Task<List<TransactionResponseDto>> GetTransactionsAsync(Guid userId, string stockTicker);

        public Task<decimal> CalculateCurrentYieldForUser(Guid userId, string stockTicker, string Data);
        public decimal CalculatePortfolioRisk(List<CalculationDTOs> stocks);
        public Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
        public Task<decimal> PercentageChange(string stockTicker, string data);
        public bool IsValidMarketPrice(decimal currentBalance);

    }
}