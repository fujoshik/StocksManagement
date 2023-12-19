using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using StockAPI.Infrastructure.Models;

public class DailyYieldChangesService : IDailyYieldChanges
{
    private readonly IHttpClientService httpClientService;

    public DailyYieldChangesService(IHttpClientService httpClientService)
    {
        this.httpClientService = httpClientService;
    }

    public async Task<List<DailyYieldChangeDto>> CalculateDailyYieldChanges(Guid accountId, string stockTicker, DateTime startDate, DateTime endDate, List<Stock> stockList)
    {
        List<DailyYieldChangeDto> dailyYieldChanges = new List<DailyYieldChangeDto>();

        var transactions = await httpClientService.GetTransactions(accountId, stockTicker);

        var firstStock = stockList.FirstOrDefault();

        foreach (var transaction in transactions)
        {
            var purchasePrice = firstStock?.ClosestPrice ?? 0m;

            var correspondingStock = stockList.FirstOrDefault(stock =>
                DateTime.TryParse(stock.Date, out var stockDate) && transaction.Date == stockDate.ToString("yyyy-MM-dd"));

            if (correspondingStock != null)
            {
                var dailyYieldChange = new DailyYieldChangeDto
                {
                    Date = transaction.Date,
                    StockTicker = stockTicker,
                    TransactionType = transaction.TransactionType,
                    Quantity = transaction.Quantity,
                    PurchasePrice = purchasePrice,
                    CurrentPrice = correspondingStock.ClosestPrice ?? 0m, 
                    DailyYield = ((correspondingStock.ClosestPrice ?? 0m) - purchasePrice) * transaction.Quantity
                };

                dailyYieldChanges.Add(dailyYieldChange);
            }
        }

        return dailyYieldChanges;
    }
}