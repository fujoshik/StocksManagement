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
        Task ExequteSqliteCommand(string commandText, SqliteConnection connection,
            Dictionary<string, object> parameters);
        SqliteConnection GetConnection();
        void Dispose();
        Task InsertStockIntoDatabase(Stock stock);
        Task InitializeDatabase();
        Task InitializeBrokerTable();
    }
}
