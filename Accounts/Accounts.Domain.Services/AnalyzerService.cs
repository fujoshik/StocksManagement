using Accounts.Domain.Abstraction.Clients;
using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Analyzer;

namespace Accounts.Domain.Services
{
    public class AnalyzerService : IAnalyzerService
    {
        private readonly IUserDetailsProvider _userDetailsProvider;
        private readonly IAnalyzerClient _analyzerClient;
        private readonly IWalletService _walletService;

        public AnalyzerService(IUserDetailsProvider userDetailsProvider,
                               IAnalyzerClient analyzerClient,
                               IWalletService walletService)
        {
            _userDetailsProvider = userDetailsProvider;
            _analyzerClient = analyzerClient;
            _walletService = walletService;
        }

        public async Task<CalculateCurrentYieldDto> CalculateCurrentYieldAsync(string ticker, string date)
        {
            var accountId = _userDetailsProvider.GetAccountId();

            return await _analyzerClient.CalculateAverageIncomeForPeriodAsync(accountId, ticker, date);
        }

        public async Task<PercentageChangeDto> GetPercentageChangeAsync(string ticker, string date)
        {
            var wallet = await _walletService.GetWalletInfoAsync(default(Guid));

            return await _analyzerClient.GetPercentageChangeAsync(wallet.Id, ticker, date);
        }

        public async Task<List<DailyYieldChangeDto>> GetDailyYieldChangesAsync(string date, string ticker)
        {
            var accountId = _userDetailsProvider.GetAccountId();

            return await _analyzerClient.GetDailyYieldChangesAsync(date, ticker, accountId);
        }
    }
}
