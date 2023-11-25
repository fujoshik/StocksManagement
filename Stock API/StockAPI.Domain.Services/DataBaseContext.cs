using Microsoft.Data.Sqlite;
using Serilog;
using SQLitePCL;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services
{
    public class DataBaseContext:IDataBaseContext
    {
        private readonly SqliteConnection _connection;

        public DataBaseContext(string connectionString)
        {
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
        }

        public SqliteConnection GetConnection()
        {
            return _connection;
        }

        public void InitializeDatabase()
        {
            try
            {
                using (var command = new SqliteCommand("CREATE TABLE IF NOT EXISTS Stocks " +
                "(StockTicker TEXT, ClosestPrice REAL, HighestPrice REAL, LowestPrice REAL, " +
                "TransactionCount INTEGER, OpenPrice REAL, IsOTC INTEGER, UnixTimestamp INTEGER," +
                " TradingVolume INTEGER, VolumeWeightedAveragePrice REAL, Date TEXT)", _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, "an exception occured while trying to initialize tha database.");
                throw;
            }
        }

        public void InsertStockIntoDatabase(Stock stock)
        {
            try
            {
                using (var command = new SqliteCommand("INSERT INTO Stocks (StockTicker, " +
                "ClosestPrice, HighestPrice, LowestPrice, TransactionCount, OpenPrice, " +
                "IsOTC, UnixTimestamp, TradingVolume, VolumeWeightedAveragePrice, Date) " +
                "VALUES (@StockTicker, @ClosestPrice, @HighestPrice, @LowestPrice, " +
                "@TransactionCount, @OpenPrice, @IsOTC, @UnixTimestamp, @TradingVolume, " +
                "@VolumeWeightedAveragePrice, @Date)", _connection))
                {
                    command.Parameters.AddWithValue("@StockTicker", stock.StockTicker);
                    command.Parameters.AddWithValue("@ClosestPrice", stock.ClosestPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@HighestPrice", stock.HighestPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LowestPrice", stock.LowestPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TransactionCount", stock.TransactionCount ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@OpenPrice", stock.OpenPrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@IsOTC", stock.IsOTC ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@UnixTimestamp", stock.UnixTimestamp ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@TradingVolume", stock.TradingVolume ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@VolumeWeightedAveragePrice", stock.VolumeWeightedAveragePrice ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Date", stock.Date);
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to add the stock {stock.StockTicker} from {stock.Date}.");
                throw;
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
