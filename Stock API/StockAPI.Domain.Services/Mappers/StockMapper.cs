using AutoMapper;
using Microsoft.Data.Sqlite;
using StockAPI.Domain.Abstraction.Mappers;
using StockAPI.Infrastructure.Models;
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
        public Stock ResultToStock (Result item, string currentDate)
        {
            return new Stock
            {
                StockTicker = item.T,
                ClosestPrice = item.c,
                HighestPrice = item.h,
                LowestPrice = item.l,
                TransactionCount = item.n,
                OpenPrice = item.o,
                IsOTC = item.otc,
                UnixTimestamp = item.t,
                TradingVolume = item.v,
                VolumeWeightedAveragePrice = item.vw,
                Date = currentDate
            };
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
                IsOTC = reader["IsOTC"] == DBNull.Value ? null : (bool?)(reader["IsOTC"]),
                UnixTimestamp = reader["UnixTimestamp"] == DBNull.Value ? null : (long?)Convert.ToInt64(reader["UnixTimestamp"]),
                TradingVolume = reader["TradingVolume"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["TradingVolume"]),
                VolumeWeightedAveragePrice = reader["VolumeWeightedAveragePrice"] == DBNull.Value ? null : (double?)Convert.ToDouble(reader["VolumeWeightedAveragePrice"]),
                Date = reader["Date"].ToString()
            };
        }
    }
}
