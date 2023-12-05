using Analyzer.API.Analyzer.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IDailyYieldChanges
    {
        Task<decimal> CalculateDailyYieldChanges(List<CalculationDTOs> stocks);
    }

}
