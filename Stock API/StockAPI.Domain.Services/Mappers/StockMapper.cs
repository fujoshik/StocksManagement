using AutoMapper;
using Microsoft.Data.Sqlite;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Infrastructure.Models;
using StockAPI.Infrastructure.Models.FillData;
using StockAPI.Infrastructure.Models.GetGroupedDaily;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services.Mappers
{
    public class StockMapper:IStockMapper
    {
        private readonly IMapper _mapper;
        public StockMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Stock ResultToStock (Result item, string currentDate)
        {
            var stock = _mapper.Map<Stock>(item);
            stock.Date = currentDate;
            return stock;
        }

        public Stock StockByDateAndTickerRootToStock(StockByDateAndTickerRoot item, string currentDate)
        {
            var stock = _mapper.Map<Stock>(item);
            stock.Date = currentDate;
            return stock;
        }

        public Stock DataToStock(SqliteDataReader reader)
        {
            return new Stock
            {
                StockTicker = reader["StockTicker"].ToString(),
                ClosestPrice = reader["ClosestPrice"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["ClosestPrice"]),
                HighestPrice = reader["HighestPrice"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["HighestPrice"]),
                LowestPrice = reader["LowestPrice"] == DBNull.Value ? null : (decimal?)Convert.ToDecimal(reader["LowestPrice"]),
                TransactionCount = reader["TransactionCount"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TransactionCount"]),
                OpenPrice = reader["OpenPrice"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["OpenPrice"]),
                IsOTC = reader["IsOTC"] == DBNull.Value ? null : (bool?)(Convert.ToInt64(reader["IsOTC"]) != 0),
                UnixTimestamp = reader["UnixTimestamp"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["UnixTimestamp"]),
                TradingVolume = reader["TradingVolume"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TradingVolume"]),
                VolumeWeightedAveragePrice = reader["VolumeWeightedAveragePrice"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["VolumeWeightedAveragePrice"]),
                Date = reader["Date"].ToString()
            };
        }

        public Stock TimeSeriesToStock(TimeSeries item, string symbol, string date)
        {
            var stock = _mapper.Map<Stock>(item);
            stock.StockTicker = symbol;
            stock.Date = date;
            stock.VolumeWeightedAveragePrice = null;
            stock.UnixTimestamp = null;
            return stock;
        }
    }
}
