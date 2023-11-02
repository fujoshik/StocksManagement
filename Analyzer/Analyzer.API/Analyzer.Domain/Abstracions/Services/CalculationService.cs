﻿using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class CalculationService : ICalculationService
    {
        public async Task<decimal> CalculateCurrentYield(Guid id, decimal initialBalance, decimal currentBalance)
        {
            if (currentBalance <= 0)
            {
                throw new ArgumentException("Invalid current market price.");
            }

            decimal currentYield = (initialBalance / currentBalance) * 100;
            return currentYield;
        }

        public bool IsValidMarketPrice(decimal currentBalance)
        {
            return currentBalance > 0;
        }

    }
}