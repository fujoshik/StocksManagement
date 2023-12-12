using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.API.Analyzer.Domain.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Analyzer.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class DailyYieldChangesService : IDailyYieldChanges
    {
        private readonly IHttpClientService httpClientService;

        public DailyYieldChangesService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public List<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stockData)
        {
            List<decimal> dailyYieldChanges = new List<decimal>();

            for (int i = 1; i < stockData.Count; i++)
            {
                decimal currentStockPrice = stockData[i].StockPrice;
                decimal previousStockPrice = stockData[i - 1].StockPrice;

                decimal dailyYieldChange = ((currentStockPrice - previousStockPrice) / previousStockPrice) * 100;

                dailyYieldChanges.Add(dailyYieldChange);
            }

            return dailyYieldChanges;
        }
    }
}