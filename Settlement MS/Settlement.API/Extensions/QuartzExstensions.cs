using Quartz;
using Settlement.Domain;
using Settlement.Domain.Constants;

namespace Settlement.API.Extensions
{
    public static class QuartzExstensions
    {
        public static void AddQuartzConfiguration(IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var dailySettlementJob = JobKey.Create(nameof(DailySettlementJob));
                q.AddJob<DailySettlementJob>(dailySettlementJob)
                    .AddTrigger(t => t
                        .ForJob(dailySettlementJob)
                        .WithCronSchedule(CronExpressionConstant.CronExpression));

                var processFailedTransactions = JobKey.Create(nameof(ProcessFailedTransactionsJob));
                q.AddJob<ProcessFailedTransactionsJob>(processFailedTransactions)
                    .AddTrigger(t => t
                       .ForJob(processFailedTransactions)
                       .WithCronSchedule(CronExpressionConstant.CronExpression));
            });

            services.AddQuartzHostedService();
        }
    }
}
