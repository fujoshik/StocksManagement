using Accounts.Domain.Jobs;
using Accounts.Infrastructure.Mapper;
using Quartz;

namespace Accounts.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStocksAutomapper(this IServiceCollection services)
            => services.AddAutoMapper(mc =>
            {
                mc.AddProfile(new AccountProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new WalletProfile());
            });

        public static void AddQuartzConfiguration(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                var jobKey = JobKey.Create(nameof(TrialBackgroundJob));
                options
                    .AddJob<TrialBackgroundJob>(jobKey)
                    .AddTrigger(trigger => 
                        trigger
                            .ForJob(jobKey)
                            .WithCalendarIntervalSchedule(schedule =>
                                schedule
                                    .WithIntervalInDays(30))
                                    .EndAt(DateBuilder.FutureDate(1, IntervalUnit.Month)));
            });

            services.AddQuartzHostedService();
        }
    }
}
