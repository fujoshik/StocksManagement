using System;
using System.Collections.Generic;
using System.Linq;
using Analyzer.API.Analyzer.Domain.DTOs;
using Accounts.Domain.Enums;
using Analyzer.Domain.DTOs;

public class Calculator
{
    public decimal CalculateAnnualIncome(List<TransactionResponseDto> dividendTransactions)
    {
        decimal annualIncome = dividendTransactions
            .Where(t => t.TransactionType == TransactionType.Sold)
            .Sum(t => t.Price);

        return annualIncome;
    }
}