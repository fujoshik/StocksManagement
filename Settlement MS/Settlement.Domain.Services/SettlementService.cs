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

                bool transactionSucceeded = userAccountBalance.CurrentBalance >= tradeCommission;

                if (transactionSucceeded)
                {
                    if (transactionRequest.TransactionType == Enums.TransactionType.Bought)
                    {
                        userAccountBalance.CurrentBalance -= tradeCommission;
                    }
                    else
                    {
                        userAccountBalance.CurrentBalance += tradeCommission;
                    }

                    response.Success = true;
                    response.Message = ResponseMessagesConstants.AccountInGoodStanding;
                    response.TotalBalance = userAccountBalance.CurrentBalance;
                    response.StockPrice = closestPrice;
                    transactionRequest.StockPrice = closestPrice;

                    await settlementRepository.UpdateWalletBalance(transactionRequest.WalletId, userAccountBalance.CurrentBalance);
                    await settlementRepository.InsertTransaction(transactionRequest, response);
                    await settlementRepository.InsertHandledWallets(transactionRequest.WalletId);
                }
                else
                {
                    response.Success = false;
                    response.Message = ResponseMessagesConstants.InsufficientFunds;
                    response.TotalBalance = userAccountBalance.CurrentBalance;
                    response.StockPrice = closestPrice;
                    transactionRequest.StockPrice = closestPrice;

                    await settlementRepository.InsertTransaction(transactionRequest, response);
                }
            }
            else
            {
                response.Success = false;
                response.Message = ResponseMessagesConstants.ErrorProcessingDeal;
            }

            return response;
        }

    }
}