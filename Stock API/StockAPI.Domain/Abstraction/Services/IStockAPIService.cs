using StockAPI.Infrastructure.Models;
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

        Task<string> Test();
    }
}
