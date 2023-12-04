using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models.FillData
{
    public class StockData
    {
        [JsonProperty("Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<string, TimeSeries> TimeSeriesDaily { get; set; }

        [JsonProperty("Weekly Time Series")]
        public Dictionary<string, TimeSeries> TimeSeriesWeekly { get; set; }

        [JsonProperty("Monthly Time Series")]
        public Dictionary<string, TimeSeries> TimeSeriesMonthly { get; set; }
    }
}
