using FluentScheduler;
using StockAPI.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services.Scheduling
{
    public class MyRegistry:Registry
    {
        private IStockAPIService _stockAPIService { get; set; }
        public MyRegistry(IStockAPIService stockAPIService)
        {
            _stockAPIService = stockAPIService;
            Schedule(() => new DailyJob(_stockAPIService)).ToRunEvery(1).Days().At(00, 00);
        }
    }
}
