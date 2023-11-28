using Quartz;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Services;

public class ProcessFailedTransactionsJob : IJob
{
    private readonly ISettlementRepository settlementRepository;
    private readonly ISettlementService settlementService;

    public ProcessFailedTransactionsJob(ISettlementRepository settlementRepository, ISettlementService settlementService)
    {
        this.settlementRepository = settlementRepository;
        this.settlementService = settlementService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var failedTransactions = await settlementRepository.GetFailedTransactions();

        foreach (var failedTransaction in failedTransactions)
        {
            var isProcessed = await settlementService.ExecuteDeal(failedTransaction);

            if (isProcessed.Success)
            {
                await settlementRepository.DeleteFailedTransaction(failedTransaction.WalletId);
            }
        }
    }
}