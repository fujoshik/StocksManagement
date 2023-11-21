using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly IHttpClientService httpClientService;

        public SettlementService(IHttpClientService httpClientService)
        {
            this.httpClientService = httpClientService;
        }

        public async Task<SettlementResponseDto> ExecuteDeal(TransactionRequestDto transactionRequest)
        {
            SettlementResponseDto response = new SettlementResponseDto();

            var userAccountBalance = await httpClientService.GetWalletBalance(transactionRequest.WalletId);

            var stockData = await httpClientService.GetStockByDateAndTicker(transactionRequest.Date, transactionRequest.StockTicker);

            if (stockData != null && stockData.ClosestPrice.HasValue)
            {
                decimal closestPrice = stockData.ClosestPrice.Value;
                decimal commissionPercentage = 0.0005M;
                decimal tradeCommission = (closestPrice * transactionRequest.Quantity) * commissionPercentage;

                if (userAccountBalance.CurrentBalance >= tradeCommission)
                {
                    userAccountBalance.CurrentBalance -= tradeCommission;
                    response.Success = true;
                    response.Message = "Your account is in good standing.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "Insufficient funds to complete the transaction.";
                }

                response.StockPrice = closestPrice;
            }
            else
            {
                response.Success = false;
                response.Message = "Closest Price is null. Error while processing the deal.";
            }

            response.TotalBalance = userAccountBalance.CurrentBalance;

            return response;
        }
    }
}