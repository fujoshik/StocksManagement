using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.Request
{
    public class RequestLogDTO
    {
        public string UserId { get; set; }
        public string Route { get; set; }
        public DateTime Timestamp { get; set; }
        
    }

}
