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
        private readonly IWalletCacheService walletCacheService;
        public SettlementService(IHttpClientService httpClientService, ISettlementRepository settlementRepository,
            IWalletCacheService walletCacheService)
        {
            this.httpClientService = httpClientService;
            this.settlementRepository = settlementRepository;
            this.walletCacheService = walletCacheService;
        }

        public async Task<SettlementResponseDto> ExecuteDeal(TransactionRequestDto transactionRequest)
        {
            SettlementResponseDto response = new SettlementResponseDto();

            var stockData = await httpClientService.GetStockByDateAndTicker(transactionRequest.Date, transactionRequest.StockTicker);

            if (stockData != null && stockData.ClosestPrice.HasValue)            
            {
                decimal closestPrice = stockData.ClosestPrice.Value;
                decimal tradeCommission = (closestPrice * transactionRequest.Quantity) * CommissionPercentageConstant.commissionPercentage;

                transactionRequest.StockPrice = closestPrice;

                var userAccountBalance = await walletCacheService.GetWalletFromCache(transactionRequest.WalletId);

                if(userAccountBalance == null)
                {
                    userAccountBalance = await httpClientService.GetWalletBalance(transactionRequest.WalletId, transactionRequest);
                }

                bool isValidWalletId = await settlementRepository.CheckValidWalletId(transactionRequest.WalletId);

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

                    if(isValidWalletId)
                    {
                        await settlementRepository.UpdateWalletBalance(transactionRequest.WalletId, userAccountBalance.CurrentBalance);
                        await settlementRepository.InsertTransaction(transactionRequest, response);
                        await settlementRepository.InsertHandledWallets(transactionRequest.WalletId, transactionRequest.AccountId, 
                            transactionRequest.Id);
                        await settlementRepository.UpdateTransaction(transactionRequest);
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = ResponseMessagesConstants.InsufficientFunds;
                    response.TotalBalance = userAccountBalance.CurrentBalance;
                    response.StockPrice = closestPrice;

                    if(isValidWalletId)
                    {
                        await settlementRepository.InsertTransaction(transactionRequest, response);
                        await settlementRepository.UpdateTransaction(transactionRequest);
                    }
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