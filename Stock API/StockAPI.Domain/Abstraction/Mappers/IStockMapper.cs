using Microsoft.Data.Sqlite;
using StockAPI.Infrastructure.Models;
using StockAPI.Infrastructure.Models.FillData;
using StockAPI.Infrastructure.Models.GetGroupedDaily;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Abstraction.Mappers
{
    public interface IStockMapper
    {
        Stock ResultToStock(Result item, string currentDate);
        Stock StockByDateAndTickerRootToStock(StockByDateAndTickerRoot item, string currentDate);
        Stock TimeSeriesToStock(TimeSeries item, string symbol, string date);
        Stock DataToStock(SqliteDataReader reader);
    }
}
