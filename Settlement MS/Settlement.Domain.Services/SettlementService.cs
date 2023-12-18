using Accounts.Domain.DTOs.Wallet;
using Settlement.Domain.Abstraction.Services;
using Settlement.Domain.Calculation;
using Settlement.Domain.Constants.Messages;
using Settlement.Domain.DTOs.Settlement;
using Settlement.Domain.DTOs.Transaction;
using Settlement.Domain.Enums;
using StockAPI.Infrastructure.Models;

namespace Settlement.Domain.Services
{
    public class SettlementService : ISettlementService
    {
        private readonly ISettlementDependencies dependencies;
        public SettlementService(ISettlementDependencies dependencies)
        {
            this.dependencies = dependencies;
        }

        public async Task<SettlementResponseDto> ExecuteDeal(TransactionRequestDto transactionRequest)
        {
            SettlementResponseDto response = new SettlementResponseDto();

            response = await ProcessTransaction(transactionRequest, response);

            if (!response.Success)
            {
                SetErrorResponseData(response, false, ResponseMessagesConstants.ErrorProcessingDeal);
            }

            return response;
        }
        private bool IsValidStockData(Stock stockData)
        {
            return stockData != null && stockData.ClosestPrice.HasValue;
        }

        private decimal CalculateTradeCommission(decimal closestPrice, decimal quantity, decimal commissionPercentage)
        {
            return (closestPrice * quantity) * commissionPercentage;
        }
        private async Task<Stock> GetStockData(string date, string stockTicker)
        {
            return await dependencies.Http.GetStockByDateAndTicker(date, stockTicker);
        }

        private bool IsTransactionSuccessful(WalletResponseDto userAccountBalance, decimal tradeCommission)
        {
            return userAccountBalance.CurrentBalance >= tradeCommission;
        }

        private async Task<WalletResponseDto> GetWalletData(Guid walletId, TransactionRequestDto transaction)
        {
            var walletBalance = await dependencies.Cache.GetWalletFromCache(walletId);

            if (walletBalance == null)
            {
                walletBalance = await dependencies.Http.GetWalletBalance(walletId, transaction);
            }

            return walletBalance;
        }

        private void AdjustBalance(TransactionType transactionType, WalletResponseDto wallet, decimal tradeCommission)
        {
            if (transactionType == TransactionType.Bought)
            {
                wallet.CurrentBalance -= tradeCommission;
            }
            else
            {
                wallet.CurrentBalance += tradeCommission;
            }
        }

        private void SetResponseData(SettlementResponseDto response, bool success, string message, decimal totalBalance, decimal stockPrice)
        {
            response.Success = success;
            response.Message = message;
            response.TotalBalance = totalBalance;
            response.StockPrice = stockPrice;
        }

        private void SetErrorResponseData(SettlementResponseDto response, bool success, string message)
        {
            response.Success = success;
            response.Message = message;
        }

        private async Task HandleTransaction(TransactionRequestDto transaction, WalletResponseDto wallet, SettlementResponseDto settlement)
        {
            if(settlement.Success)
            {
                await dependencies.Repository.UpdateWalletBalance(transaction.WalletId, wallet.CurrentBalance);
                await dependencies.Repository.InsertTransaction(transaction, settlement);
                await dependencies.Repository.InsertHandledWallets(transaction.WalletId, transaction.AccountId, transaction.Id);
                await dependencies.Repository.UpdateTransaction(transaction);
            }
            else
            {
                await dependencies.Repository.InsertTransaction(transaction, settlement);
                await dependencies.Repository.UpdateTransaction(transaction);
            }
        }

        private async Task<SettlementResponseDto> ProcessTransaction(TransactionRequestDto transactionRequest, SettlementResponseDto response)
        {
            var stockData = await GetStockData(transactionRequest.Date, transactionRequest.StockTicker);

            if (!IsValidStockData(stockData))
            {
                SetErrorResponseData(response, false, ResponseMessagesConstants.ErrorProcessingDeal);
                return response;
            }

            decimal closestPrice = stockData.ClosestPrice.Value;

            decimal commissionPercentage = CommisionCalculator.GetCommisionPercentage(transactionRequest.Role);

            decimal tradeCommission = CalculateTradeCommission(closestPrice, transactionRequest.Quantity, commissionPercentage);

            transactionRequest.StockPrice = closestPrice;

            var userAccountBalance = await GetWalletData(transactionRequest.WalletId, transactionRequest);

            bool isValidWalletId = await dependencies.Repository.CheckValidWalletId(transactionRequest.WalletId);

            bool transactionSucceeded = IsTransactionSuccessful(userAccountBalance, tradeCommission);

            if (transactionSucceeded)
            {
                AdjustBalance(transactionRequest.TransactionType, userAccountBalance, tradeCommission);

                SetResponseData(response, true, ResponseMessagesConstants.AccountInGoodStanding, userAccountBalance.CurrentBalance, closestPrice);

                if (isValidWalletId)
                {
                    await HandleTransaction(transactionRequest, userAccountBalance, response);
                }
            }
            else
            {
                SetResponseData(response, false, ResponseMessagesConstants.InsufficientFunds, userAccountBalance.CurrentBalance, closestPrice);

                if (isValidWalletId)
                {
                    await HandleTransaction(transactionRequest, userAccountBalance, response);
                }
            }

            return response;
        }
    }
}