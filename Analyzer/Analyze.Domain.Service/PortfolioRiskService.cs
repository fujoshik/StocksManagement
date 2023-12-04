using Accounts.Domain.Abstraction.Services;
using Analyzer.Domain.Abstracions.Interfaces;
using System.Threading.Tasks;
using Accounts.Domain.DTOs.Wallet;

namespace Analyze.Domain.Service
{
    public class PortfolioRiskService : IPortfolioRiskService
    {
        private readonly IAccountService accountService;
        private readonly IStockService stockService;

        public PortfolioRiskService(IAccountService accountService, IStockService stockService)
        {
            this.accountService = accountService;
            this.stockService = stockService;
        }

        public async Task<decimal> CalculatePortfolioRisk(Guid accountId, string stockTicker, DateTime date)
        {
          

            return 0; 
        }
    }
}

