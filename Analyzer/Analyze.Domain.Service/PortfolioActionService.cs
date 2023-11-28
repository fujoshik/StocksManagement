using Accounts.Domain.Abstraction.Services;
using Analyzer.Domain.Abstracions.Interfaces;
using Settlement.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyze.Domain.Service
{
    public class PortfolioActionService : IPortfolioActionService
    {
        private readonly IAccountService accountService;
        private readonly ISettlementService settlementService;

        public PortfolioActionService(IAccountService accountService, ISettlementService settlementService)
        {
            this.accountService = accountService;
            this.settlementService = settlementService;
        }

        public async Task CheckAndExecuteActions(Guid accountId, decimal initialInvestment, decimal currentBalance)
        {
            // Имплементирайте логиката за проверка и изпълнение на действията в портфейла тук
            // Можете да използвате IAccountService и ISettlementService за вземане на необходимите данни
            // ...

            // Пример: Проверка за загуба от 15%
            if (CheckLossExceeded(initialInvestment, currentBalance, 15))
            {
                // Изпратете съобщение на потребителя за загуба
                Console.WriteLine($"Warning: Portfolio loss exceeded 15%. Please review your investments.");
            }

            // Пример: Проверка за загуба от 20%
            if (CheckLossExceeded(initialInvestment, currentBalance, 20))
            {
                // Извикайте метод за продажба на активите и превеждане на остатъка
                await SellAssetsAndTransferFunds(accountId);
            }
        }

        private bool CheckLossExceeded(decimal initialInvestment, decimal currentBalance, int percentageThreshold)
        {
            decimal lossPercentage = ((initialInvestment - currentBalance) / initialInvestment) * 100;
            return lossPercentage >= percentageThreshold;
        }

        private async Task SellAssetsAndTransferFunds(Guid accountId)
        {
            // Извършете логиката за продажба на активите и превеждане на остатъка
            // Използвайте ISettlementService за превеждане на средствата
            // ...

            Console.WriteLine($"Assets sold, remaining funds transferred. Portfolio loss exceeded 20%.");
        }
    }
}
