using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.HistoricalData
{
    public class HistoricalDataDto
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}
