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
        Task<List<DailyYieldChangeDto>> DailyYieldChanges(Guid accountId, string stockTicker);

       // Task<List<DailyYieldChangeDto>> DailyYieldChanges(Guid accountId, string stockTicker);
        //Task<List<DailyYieldChangeDto>> CalculateAnotherDailyYieldChanges(Guid accountId, string stockTicker);
    }

}
