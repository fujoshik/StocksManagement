using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Services.AppSettings
{
    public class EndPoints
    {
        public string GroupedDaily { get; set; }
        public string DailyOpenClose { get; set; }
        public string Tickers { get; set; }
        public string DailyWeeklyMonthly { get; set; }

    }
}
