using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.Abstracions.Interfaces
{
    public interface IPortfolioActionService
    {
        Task CheckAndExecuteActions(Guid accountId, decimal initialInvestment, decimal currentBalance);
    }
}
