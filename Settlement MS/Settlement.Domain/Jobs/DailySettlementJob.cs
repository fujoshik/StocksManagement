using Quartz;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Constants;
using System.Linq;

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
            var handledWallets = await settlementRepository.GetHandledWalletIds();

            foreach (var handledWallet in handledWallets)
            {
                var assocoatedTransaction = await settlementRepository.GetTransactionById(handledWallet.TransactionId);

                if (assocoatedTransaction != null)
                {
                    var wallet = await settlementRepository.GetWalletById(handledWallet.WalletId);

                    if (wallet != null)
                    {
                        decimal tradeCommission = wallet.CurrentBalance * CommissionPercentageConstant.commissionPercentage;

                        wallet.CurrentBalance -= tradeCommission;

                        await settlementRepository.UpdateWalletBalance(wallet.Id, wallet.CurrentBalance);
                    }
                }
            }
        }

    }
}
