using Gateway.Domain.Abstraction.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Gateway.Domain.Services
{
    

    public class TradeService : ITradeService
    {
        private readonly IUserService _userService;
        private readonly ILogger<TradeService> _logger;

        public TradeService(IUserService userService, ILogger<TradeService> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        //public void ExecuteTrade(string userId, string symbol, decimal amount, TradeType type)
        //{
        //    try
        //    {
                
        //        if (!_userService.UserExists(userId))
        //        {
        //            _logger.LogWarning($"User {userId} not found. Trade execution skipped.");
        //            return;
        //        }

        //        decimal tradeValue = CalculateTradeValue(symbol, amount, type);

        //        _userService.UpdateUserBalance(userId, tradeValue);

        //        UpdateUserStatus(userId, tradeValue);

        //        _logger.LogInformation($"Trade executed for user {userId}. Symbol: {symbol}, Amount: {amount}, Type: {type}, Trade Value: {tradeValue}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error executing trade for user {userId}: {ex.Message}");
        //    }
        //}

        //private decimal CalculateTradeValue(string symbol, decimal amount, TradeType type)
        //{
        //    decimal assetValue = GetAssetValue(symbol);

        //    decimal tradeValue = 0;

        //    switch (type)
        //    {
        //        case TradeType.Buy:
        //            tradeValue = amount * assetValue;
        //            break;

        //        case TradeType.Sell:
        //            tradeValue = amount * assetValue;
        //            break;

        //        default:
        //            break;
        //    }

        //    return tradeValue;
        //}

        private decimal GetAssetValue(string symbol)
        {
            // StockAPIService.GetAssetValue(symbol);
            
            return 100;
        }
    }
}
