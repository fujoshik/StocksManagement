using Microsoft.Data.Sqlite;
using StockAPI.Infrastructure.Models;
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

        Stock DataToStock(SqliteDataReader reader);
    }
}
