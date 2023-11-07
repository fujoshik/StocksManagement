﻿using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Abstraction.Services
{
    public interface IStockAPIService
    {
        Task<List<Stock>> GetGroupedDailyData();
        Task<List<Stock>> GetAllStocks();
        Task<Stock> GetStockByDateAndTickerAsync(string date, string stockTicker);
        Task<List<Stock>> GetStocksByDate(string date);
        Task<StockMarketCharacteristics> GetStockMarketCharacteristics(string date);

    }
}
