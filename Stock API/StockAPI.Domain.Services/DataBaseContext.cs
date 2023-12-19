using Microsoft.Data.Sqlite;
using Serilog;
using SQLitePCL;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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

        //execute sqlite command
        public async Task ExequteSqliteCommand(string commandText, SqliteConnection connection,
            Dictionary<string, object> parameters)
        {
            try
            {
                using (var command = new SqliteCommand(commandText, connection))
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an exception occured while trying to execute the command.");
            }
        }

        //create the database if it doesn't exist
        public async Task InitializeDatabase()
        {
            try
            {
                using (var command = new SqliteCommand("CREATE TABLE IF NOT EXISTS Stocks " +
                "(StockTicker TEXT, ClosestPrice REAL, HighestPrice REAL, LowestPrice REAL, " +
                "TransactionCount INTEGER, OpenPrice REAL, IsOTC INTEGER, UnixTimestamp INTEGER," +
                " TradingVolume INTEGER, VolumeWeightedAveragePrice REAL, Date TEXT)", _connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                Log.Information("the database was initialized.");
            }
            catch(Exception ex)
            {
                Log.Error(ex, "an exception occured while trying to initialize the database.");
            }
        }

        //check for duplicates
        public async Task<bool> CheckForDuplicates(Stock stock)
        {
            try
            {
                using (var selectCommand = new SqliteCommand("SELECT COUNT(*) FROM Stocks " +
                "WHERE Date = @Date AND StockTicker = @StockTicker", _connection))
                {
                    selectCommand.Parameters.AddWithValue("@Date", stock.Date);
                    selectCommand.Parameters.AddWithValue("@StockTicker", stock.StockTicker);

                    int existingRecordsCount = Convert.ToInt32(await selectCommand.ExecuteScalarAsync());

                    if (existingRecordsCount > 0) 
                    {
                        Log.Information("a too recent record of the stock {@StockTicker} already exists.", stock.StockTicker);
                        return true;
                    }
                    else return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to check for duplicates ");
                return true;
            }
        }

        //insert a stock into the database
        public async Task InsertStockIntoDatabase(Stock stock)
        {
            try
            {
                bool hasDuplicates = await CheckForDuplicates(stock);

                if (!hasDuplicates)
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                        {
                            { "@StockTicker", stock.StockTicker },
                            { "@ClosestPrice", stock.ClosestPrice },
                            { "@HighestPrice", stock.HighestPrice },
                            { "@LowestPrice", stock.LowestPrice },
                            { "@TransactionCount", stock.TransactionCount },
                            { "@OpenPrice", stock.OpenPrice },
                            { "@IsOTC", stock.IsOTC },
                            { "@UnixTimestamp", stock.UnixTimestamp },
                            { "@TradingVolume", stock.TradingVolume },
                            { "@VolumeWeightedAveragePrice", stock.VolumeWeightedAveragePrice },
                            { "@Date", stock.Date }
                        };
                    await ExequteSqliteCommand("INSERT INTO Stocks (StockTicker, " +
            "ClosestPrice, HighestPrice, LowestPrice, TransactionCount, OpenPrice, " +
            "IsOTC, UnixTimestamp, TradingVolume, VolumeWeightedAveragePrice, Date) " +
            "VALUES (@StockTicker, @ClosestPrice, @HighestPrice, @LowestPrice, " +
            "@TransactionCount, @OpenPrice, @IsOTC, @UnixTimestamp, @TradingVolume, " +
            "@VolumeWeightedAveragePrice, @Date)", _connection, parameters);
                }     
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to add the stock " +
                    $"{stock.StockTicker} from {stock.Date}.");
            }
        }

        //close and dispose of connection
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
