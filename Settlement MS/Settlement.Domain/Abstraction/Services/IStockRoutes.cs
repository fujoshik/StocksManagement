using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.Abstraction.Services
{
    public interface IStockRoutes
    {
        public Dictionary<string, string> Routes { get; set; }
    }
}
