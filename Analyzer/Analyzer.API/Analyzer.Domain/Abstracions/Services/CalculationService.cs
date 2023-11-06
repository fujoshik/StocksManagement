using Accounts.Domain.DTOs.Wallet;
using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IHttpClientService httpClientAccounts;

        public CalculationService(IHttpClientService httpClientAccounts)
        {
            this.httpClientAccounts = httpClientAccounts;
        }

        public decimal CalculateCurrentYield(WalletResponseDto walletResponseDto)
        {
            decimal initialBalance = walletResponseDto.InitialBalance;
            decimal currentBalance = walletResponseDto.CurrentBalance;

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
