using StockAPI.Infrastructure.Enums;
using StockAPI.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Domain.Abstraction.Services
{
    public interface IFillDatabaseService
    {
        Task<List<string>> GetTickersList();
        Task<List<Stock>> FillData(DataOption dataOption, string symbol);
    }
}
