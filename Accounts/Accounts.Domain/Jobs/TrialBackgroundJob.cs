using Microsoft.Extensions.Logging;
using Quartz;

namespace Accounts.Domain.Jobs
{
    public class TrialBackgroundJob : IJob
    {
        private readonly ILogger<TrialBackgroundJob> _logger;

        public TrialBackgroundJob(ILogger<TrialBackgroundJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("");
            return Task.CompletedTask;
        }
    }
}
