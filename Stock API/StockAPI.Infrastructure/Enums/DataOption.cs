using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Enums
{
    public enum DataOption
    {
        [EnumMember(Value = "DAILY")]
        DAILY,

        [EnumMember(Value = "WEEKLY")]
        WEEKLY,

        [EnumMember(Value = "MONTHLY")]
        MONTHLY
    }
}
