using Microsoft.Data.Sqlite;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Abstraction.DataBase
{
    public interface IDataBaseContext
    {
        SqliteConnection GetConnection();
        void Dispose();
        void InsertStockIntoDatabase(Stock stock);
        void InitializeDatabase();
    }
}
