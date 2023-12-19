using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Services;

namespace Accounts.Domain.Services
{
    public class AnalyzerService : IAnalyzerService
    {
        private readonly IUserDetailsProvider _userDetailsProvider;
        private readonly IAnalyzerClient _analyzerClient;

        public AnalyzerService(IUserDetailsProvider userDetailsProvider,
                               IAnalyzerClient analyzerClient)
        {
            _userDetailsProvider = userDetailsProvider;
            _analyzerClient = analyzerClient;
        }

        public async Task<decimal> CalculateAverageIncomeForPeriodAsync(string ticker, string date)
        {
            var accountId = _userDetailsProvider.GetAccountId();

            return await _analyzerClient.CalculateAverageIncomeForPeriodAsync(accountId, ticker, date);
        }
    }
}
