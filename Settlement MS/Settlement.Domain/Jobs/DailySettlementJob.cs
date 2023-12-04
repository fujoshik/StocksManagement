using Quartz;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Constants;

namespace Settlement.Domain
{
    public class DailySettlementJob : IJob
    {
        private readonly ISettlementRepository settlementRepository;
        public DailySettlementJob(ISettlementRepository settlementRepository)
        {
            this.settlementRepository = settlementRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var wallets = await settlementRepository.GetAllWallets();
            var handledWallets = await settlementRepository.GetHandledWalletIds();

            foreach (var wallet in wallets)
            {
                if (handledWallets.Contains(wallet.Id))
                {
                    decimal tradeCommission = wallet.CurrentBalance * CommissionPercentageConstant.commissionPercentage;

                    wallet.CurrentBalance -= tradeCommission;

                    await settlementRepository.UpdateWalletBalance(wallet.Id, wallet.CurrentBalance);
                }
            }
        }

    }
}
