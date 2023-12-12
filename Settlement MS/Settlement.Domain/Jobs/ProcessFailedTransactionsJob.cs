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
            var wallet = await settlementRepository.GetWalletById(failedTransaction.WalletId);
            var transaction = await settlementRepository.GetTransactionById(failedTransaction.Id);
            if(wallet == null && transaction == null)
            {
                var originalWalletId = failedTransaction.WalletId;
                var associatedAccount = await settlementRepository.GetAccountById(failedTransaction.AccountId);
                if(associatedAccount != null)
                {
                    failedTransaction.WalletId = associatedAccount.WalletId;
                    await settlementService.ExecuteDeal(failedTransaction);
                    await settlementRepository.DeleteFailedTransaction(originalWalletId);
                }
            }
        }
    }
}