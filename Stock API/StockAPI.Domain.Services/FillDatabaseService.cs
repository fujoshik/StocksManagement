using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using StockAPI.Domain.Abstraction.DataBase;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Domain.Abstraction.Services;
using StockAPI.Domain.Services.AppSettings;
using StockAPI.Infrastructure.Enums;
using StockAPI.Infrastructure.Models;
using StockAPI.Infrastructure.Models.FillData;
using StockAPI.Infrastructure.Models.GetGroupedDaily;
using StockAPI.Infrastructure.Models.Tickers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services
{
    public class FillDatabaseService:IFillDatabaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _polygonApiKey;
        private readonly string _alphaVantageApiKey;
        private readonly string _tickers;
        private readonly string _dailyWeeklyMonthly;
        private readonly IDataBaseContext _dataBaseContext;
        private readonly IStockMapper _stockMapper;

        public FillDatabaseService(IHttpClientFactory httpClientFactory, IOptions<ApiKeys> apiKeys, IOptions<EndPoints> endPoints, IDataBaseContext dataBaseContext, IStockMapper stockMapper)
        {
            _alphaVantageApiKey = apiKeys.Value.AlphaVantageApiKey;
            _polygonApiKey = apiKeys.Value.PolygonApiKey;
            _httpClientFactory = httpClientFactory;
            _dataBaseContext = dataBaseContext;
            _stockMapper = stockMapper;
            _tickers = endPoints.Value.Tickers;
            _dailyWeeklyMonthly = endPoints.Value.DailyWeeklyMonthly;
        }

        //get a list of all available tickers on polygon
        public async Task<List<string>> GetTickersList()
        {
            try
            {
                string endpointUrl = string.Format(_tickers, _polygonApiKey);
                HttpResponseMessage response = await _httpClientFactory.CreateClient()
                    .GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                TickersRoot result = JsonConvert.DeserializeObject<TickersRoot>(responseData);

                var tickersList = new List<string>();

                foreach (var item in result.results)
                {
                    tickersList.Add(item.ticker);
                }

                return tickersList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "an error occurred while trying to retrieve all tickers from polygon.");
                throw;
            }
        }

        //fill database with data from alphavantage
        public async Task<List<Stock>> FillData(DataOption dataOption, string symbol)
        {
            try
            {
                string endpointUrl = string.Format(_dailyWeeklyMonthly, dataOption, symbol, _alphaVantageApiKey);
                
                HttpResponseMessage response = await _httpClientFactory.CreateClient()
                    .GetAsync(endpointUrl);
                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();

                StockData result = JsonConvert.DeserializeObject<StockData>(responseData);

                Dictionary<string, TimeSeries> timeSeries = dataOption switch
                {
                    DataOption.DAILY => result.TimeSeriesDaily,
                    DataOption.WEEKLY => result.TimeSeriesWeekly,
                    DataOption.MONTHLY => result.TimeSeriesMonthly,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var stockList = new List<Stock>();

                foreach (var kvp in timeSeries)
                {
                    string stockSymbol = result.MetaData.Symbol;
                    string date = kvp.Key;

                    var stock =_stockMapper.TimeSeriesToStock(kvp.Value, stockSymbol, date);
                  
                    if(dataOption==DataOption.DAILY)
                    {
                        await _dataBaseContext.InsertStockIntoDatabase(stock);
                    }

                    stockList.Add(stock);
                }

                return stockList;
            }
            catch(NullReferenceException ex)
            {
                return new List<Stock>();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"an error occurred while trying to retrieve {dataOption} " +
                                    $"data for '{symbol}' from alphavantage.");
                throw;
            }
        }
    }
}
