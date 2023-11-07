using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Domain.Services.Mappers;
using StockAPI.Infrastructure.Enums;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services
{
    public class StockAPIService : IStockAPIService

    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiKey;
        private readonly string _groupedDaily;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IStockMapper _stockMapper;

        public StockAPIService(IHttpClientFactory httpClientFactory, IOptions<ApiKeys> apiKeys, IOptions<EndPoints> endPoints, IDataBaseContext dataBaseContext, IStockMapper stockMapper)
        {
            _apiKey = apiKeys.Value.PolygonApiKey;
            _httpClientFactory = httpClientFactory;
            _groupedDaily = endPoints.Value.GroupedDaily;
            _dataBaseContext = dataBaseContext;
            _stockMapper = stockMapper;
        }

        public async Task<List<Stock>> GetGroupedDailyData()
        {
            try
            {
                string currentDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
                string endpointUrl = $"{_groupedDaily}{currentDate}?adjusted=true&apiKey={_apiKey}";

                HttpResponseMessage response = await _httpClientFactory.CreateClient()
                    .GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                Root result = JsonConvert.DeserializeObject<Root>(responseData);

                var stocks = new List<Stock>();

                foreach (var item in result.results.Take(2))
                {
                    var stock = _stockMapper.ResultToStock(item, currentDate);
                    InsertStockIntoDatabase(stock);
                    stocks.Add(stock);
                }

                Log.Information("Successfully fetched test data.");

                return stocks;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while fetching grouped daily data.");

                throw;
            }
        }

        private void InsertStockIntoDatabase(Stock stock)
        {
            _dataBaseContext.InitializeDatabase();

            using (var selectCommand = new SqliteCommand("SELECT COUNT(*) FROM Stocks " +
                "WHERE Date = @Date AND StockTicker = @StockTicker", _dataBaseContext.GetConnection()))
            {
                selectCommand.Parameters.AddWithValue("@Date", stock.Date);
                selectCommand.Parameters.AddWithValue("@StockTicker", stock.StockTicker);

                int existingRecordsCount = Convert.ToInt32(selectCommand.ExecuteScalar());

                if (existingRecordsCount > 0) Log.Information("A too recent record of the stock {@StockTicker} already exists.", stock.StockTicker);
                else
                {
                    _dataBaseContext.InsertStockIntoDatabase(stock);
                }
            }
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            List<Stock> stocks = new List<Stock>();

            try
            {
                using (var command = new SqliteCommand("SELECT * FROM Stocks", _dataBaseContext.GetConnection()))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stocks.Add(_stockMapper.DataToStock(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all stocks from the database.");
                throw;
            }

            return stocks;
        }

        public async Task<Stock> GetStockByDateAndTickerAsync(string date, string stockTicker)
        {
            Stock stock = null;

            using (var command = new SqliteCommand("SELECT * FROM Stocks WHERE Date = @Date AND StockTicker = @StockTicker", _dataBaseContext.GetConnection()))
            {
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@StockTicker", stockTicker);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        stock = _stockMapper.DataToStock(reader);
                    }
                }
            }

            return stock;
        }

        public async Task<StockMarketCharacteristics> GetStockMarketCharacteristics(string date)
        {
            DateTime inputDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime hundredDaysAgoDateTime = inputDate.AddDays(-10);
            string hundredDaysAgo = hundredDaysAgoDateTime.ToString("yyyy-MM-dd");

            await Console.Out.WriteLineAsync(date);
            await Console.Out.WriteLineAsync(hundredDaysAgo);

            List<Stock> hundredDaysAgoStocks = await GetStocksByDate(hundredDaysAgo);
            List<Stock> inputDateStocks = await GetStocksByDate(date);

            decimal? hundredDaysAgoClosestAverage = hundredDaysAgoStocks.Average(i => i.ClosestPrice);
            decimal? inputDateClosestAverage = inputDateStocks.Average(i => i.ClosestPrice);

            if (!hundredDaysAgoClosestAverage.HasValue || !inputDateClosestAverage.HasValue)
            {
                Log.Information($"not enough stock data for the period from '{hundredDaysAgo}' to '{date}'");
                throw new Exception("Not enough stock data for this period");
            }

            decimal difference = Math.Abs(inputDateClosestAverage.Value - hundredDaysAgoClosestAverage.Value);
            decimal largerAverage = Math.Max(inputDateClosestAverage.Value, hundredDaysAgoClosestAverage.Value);

            decimal percentageDifference = largerAverage != 0 ? (difference / largerAverage) * 100 : 0;

            StockMarketCharacteristics stockMarketCharacteristics = new StockMarketCharacteristics();
            if (inputDateClosestAverage.Value > hundredDaysAgoClosestAverage.Value)
            {
                stockMarketCharacteristics.MarketTrend = MarketTrend.Bull;
            }
            else if (inputDateClosestAverage.Value < hundredDaysAgoClosestAverage.Value)
            {
                stockMarketCharacteristics.MarketTrend = MarketTrend.Bear;
            }

            stockMarketCharacteristics.PercentageDifference = percentageDifference;

            return stockMarketCharacteristics;
        }

        public async Task<List<Stock>> GetStocksByDate(string date)
        {
            List<Stock> stocks = new List<Stock>();

            using (var command = new SqliteCommand("SELECT * FROM Stocks WHERE Date = @Date", _dataBaseContext.GetConnection()))
            {
                command.Parameters.AddWithValue("@Date", date);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        stocks.Add(_stockMapper.DataToStock(reader));
                    }
                }
            }

            return stocks;
        }

    }
}
