using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Enums
{
    public enum DataOption
    {
        [Description("Daily")]
        DAILY,

        [Description("Weekly")]
        WEEKLY,

        [Description("Monthly")]
        MONTHLY
    }
}
