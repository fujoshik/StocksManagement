using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Interfaces
{
    public interface ICalculationService
    {
        public decimal CalculateCurrentYield(WalletResponseDto walletResponseDto);
        bool IsValidMarketPrice(decimal currentBalance);
    }
}
