using FluentScheduler;
using Serilog;
using StockAPI.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace StockAPI.Domain.Services.Scheduling
{
    public class DailyJob:IJob
    {
        private readonly IStockAPIService _stockAPIService;
        public DailyJob(IStockAPIService stockAPIService)
        {
            _stockAPIService = stockAPIService;
        }
        public void Execute()
        {
            try
            {
                var stocks = _stockAPIService.GetGroupedDailyData().GetAwaiter().GetResult();
                Log.Information("adding daily data to the database was successful.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "adding daily data to the database was unsuccessful.");
            }
        }

    }
}
