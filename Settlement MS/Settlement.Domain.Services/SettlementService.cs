using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Constants;
using Settlement.Domain.Constants.Messages;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly IHttpClientService httpClientService;
        private readonly ISettlementRepository settlementRepository;
        public SettlementService(IHttpClientService httpClientService, ISettlementRepository settlementRepository)
        {
            this.httpClientService = httpClientService;
            this.settlementRepository = settlementRepository;
        }

        public async Task<SettlementResponseDto> ExecuteDeal(TransactionRequestDto transactionRequest)
        {
            SettlementResponseDto response = new SettlementResponseDto();

            var userAccountBalance = await httpClientService.GetWalletBalance(transactionRequest.WalletId);

            var stockData = await httpClientService.GetStockByDateAndTicker(transactionRequest.Date, transactionRequest.StockTicker);

            if (stockData != null && stockData.ClosestPrice.HasValue)
            {
                decimal closestPrice = stockData.ClosestPrice.Value;
                decimal tradeCommission = (closestPrice * transactionRequest.Quantity) * CommissionPercentageConstant.commissionPercentage;

                if (userAccountBalance.CurrentBalance >= tradeCommission)
                {
                    userAccountBalance.CurrentBalance -= tradeCommission;
                    response.Success = true;
                    response.Message = ResponseMessagesConstants.AccountInGoodStanding;
                }
                else
                {
                    response.Success = false;
                    response.Message = ResponseMessagesConstants.InsufficientFunds;
                }

                response.StockPrice = closestPrice;
            }
            else
            {
                response.Success = false;
                response.Message = ResponseMessagesConstants.ErrorProcessingDeal;
            }

            response.TotalBalance = userAccountBalance.CurrentBalance;

            await settlementRepository.InsertTransaction(transactionRequest, response); 

            await settlementRepository.UpdateWalletBalance(transactionRequest.WalletId, userAccountBalance.CurrentBalance);

            await settlementRepository.InsertHandledWallets(transactionRequest.WalletId);

            return response;
        }
    }
}