using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Settings
{
    public class HostSettings
    {
        public string Account { get; set; }
        public string StockApi { get; set; }
        public string Settlement { get; set; }
    }
}
