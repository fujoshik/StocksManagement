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
using StockAPI.Domain.Services.Scheduling;
using StockAPI.Infrastructure.Enums;
using StockAPI.Infrastructure.Models;
using StockAPI.Infrastructure.Models.GetGroupedDaily;
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
        private readonly string _dailyOpenClose;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IStockMapper _stockMapper;

        public StockAPIService(IHttpClientFactory httpClientFactory, IOptions<ApiKeys> apiKeys, IOptions<EndPoints> endPoints, IDataBaseContext dataBaseContext, IStockMapper stockMapper)
        {
            _apiKey = apiKeys.Value.PolygonApiKey;
            _httpClientFactory = httpClientFactory;
            _groupedDaily = endPoints.Value.GroupedDaily;
            _dailyOpenClose = endPoints.Value.DailyOpenClose;
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

                return stocks;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while fetching grouped daily data and adding it to the database.");
                throw;
            }
        }

        public async Task<Stock> GetStockByDateAndTickerFromAPI(string date, string stockTicker)
        {
            try
            {
                string endpointUrl = $"{_dailyOpenClose}{stockTicker}/{date}?adjusted=true&apiKey={_apiKey}";

                HttpResponseMessage response = await _httpClientFactory.CreateClient()
                    .GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                StockByDateAndTickerRoot result = JsonConvert.DeserializeObject<StockByDateAndTickerRoot>(responseData);

                var stock = new Stock();

                stock = _stockMapper.StockByDateAndTickerRootToStock(result, date);
                InsertStockIntoDatabase(stock);

                return stock;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occurred while trying to add {stockTicker} from {date}" +
                    $" to the database.");
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

                if (existingRecordsCount > 0) Log.Information("a too recent record of the stock {@StockTicker} already exists.", stock.StockTicker);
                else
                {
                    _dataBaseContext.InsertStockIntoDatabase(stock);
                    Log.Information($"successfully added {stock.StockTicker} for {stock.Date}.");
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

                Log.Information("successfully retrieved all stocks from database.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while retrieving all stocks from the database.");
                throw;
            }

            return stocks;
        }

        public async Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
        {
            try
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

                if (stock != null) Log.Information($"successfully retrieved data for the stock {stockTicker} from {date}.");
                else stock = await GetStockByDateAndTickerFromAPI(date, stockTicker);
                return stock;
            }
            catch(Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to retrieve data for the stock {stockTicker} from {date}.");
                throw;
            }
        }

        public async Task<StockMarketCharacteristics> GetStockMarketCharacteristics(string date)
        {
            try
            {
                DateTime inputDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                DateTime hundredDaysAgoDateTime = inputDate.AddDays(-10);
                string hundredDaysAgo = hundredDaysAgoDateTime.ToString("yyyy-MM-dd");

                List<Stock> hundredDaysAgoStocks = await GetStocksByDate(hundredDaysAgo);
                List<Stock> inputDateStocks = await GetStocksByDate(date);

                decimal? hundredDaysAgoClosestAverage = hundredDaysAgoStocks.Average(i => i.ClosestPrice);
                decimal? inputDateClosestAverage = inputDateStocks.Average(i => i.ClosestPrice);

                if (!hundredDaysAgoClosestAverage.HasValue || !inputDateClosestAverage.HasValue)
                {
                    Log.Information($"not enough stock data for the period from '{hundredDaysAgo}' to '{date}'.");
                    throw new Exception("Not enough stock data for this period.");
                }

                decimal difference = Math.Abs(inputDateClosestAverage.Value - hundredDaysAgoClosestAverage.Value);
                decimal largerAverage = Math.Max(inputDateClosestAverage.Value, hundredDaysAgoClosestAverage.Value);

                decimal percentageDifference = largerAverage != 0 ? (difference / largerAverage) * 100 : 0;

                StockMarketCharacteristics stockMarketCharacteristics = new StockMarketCharacteristics();

                if (inputDateClosestAverage.Value > hundredDaysAgoClosestAverage.Value)
                {
                    stockMarketCharacteristics.MarketTrend = MarketTrend.Bull.ToString();
                }
                else if (inputDateClosestAverage.Value < hundredDaysAgoClosestAverage.Value)
                {
                    stockMarketCharacteristics.MarketTrend = MarketTrend.Bear.ToString();
                }
                else stockMarketCharacteristics.MarketTrend = MarketTrend.NoChange.ToString();

                stockMarketCharacteristics.PercentageDifference = percentageDifference;
                if (percentageDifference < 20)
                {
                    if (stockMarketCharacteristics.MarketTrend == MarketTrend.Bear.ToString()) stockMarketCharacteristics.MarketTrend = MarketTrend.UnsignificantBear.ToString();
                    if (stockMarketCharacteristics.MarketTrend == MarketTrend.Bull.ToString()) stockMarketCharacteristics.MarketTrend = MarketTrend.UnsignificantBull.ToString();
                }

                Log.Information($"market characteristics were retrieved successfully for {date}.");
                return stockMarketCharacteristics;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to retrieve market characteristics for {date}.");
                throw;
            }
            
        }

        public async Task<List<Stock>> GetStocksByDate(string date)
        {
            try
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

                Log.Information($"stocks from {date} successfully retrieved.");
                return stocks;
            }
            
            catch(Exception ex)
            {
                Log.Error(ex, $"an error occured while trying to retrieve stocks from {date}.");
                throw;
            }
        }
    }
}
