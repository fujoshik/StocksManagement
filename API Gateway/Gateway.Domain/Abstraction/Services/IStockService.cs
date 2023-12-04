using API.Gateway.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IStockService
    {
        object AnalyzeStockData(string symbol);
        object GetCurrentPrice(string symbol);
        decimal GetCurrentStockValue();
        IEnumerable<HistoricalData> GetHistoricalData();
    }

}
