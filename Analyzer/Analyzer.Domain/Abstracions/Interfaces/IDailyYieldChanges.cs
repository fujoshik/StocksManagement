using Analyzer.API.Analyzer.Domain.DTOs;
using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IDailyYieldChanges
    {
        public Task<List<DailyYieldChangeDto>> CalculateDailyYieldChanges(Guid accountId, string stockTicker, DateTime startDate, DateTime endDate, List<Stock> stockList);

    }

}
