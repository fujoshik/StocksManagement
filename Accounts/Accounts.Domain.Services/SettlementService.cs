using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.ExecuteDeal;
using Accounts.Domain.DTOs.Settlement;
using Accounts.Domain.DTOs.Transaction;
using IHttpClientFactory = Accounts.Domain.Abstraction.Factories.IHttpClientFactory;

namespace Accounts.Domain.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SettlementService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SettlementResponseDto> ExecuteDealAsync(ExecuteDealDto executeDealDto)
        {
            var transactionForSettlement = new TransactionForSettlementDto()
            {
                WalletId = executeDealDto.WalletId,
                StockTicker = executeDealDto.Ticker,
                Quantity = executeDealDto.Quantity,
                TransactionType = executeDealDto.TransactionType,
                AccountId = executeDealDto.AccountId
            };

            return await _httpClientFactory.SettlementClient().ExecuteDeal(transactionForSettlement);
        }
    }
}
