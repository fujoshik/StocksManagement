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

    public async Task<List<DailyYieldChangeDto>> DailyYieldChanges(Guid accountId, string stockTicker)
    {
        try
        {
            var transactions = await httpClientService.GetTransactions(accountId, stockTicker);

            var stockPrices = await httpClientService.GetStock(stockTicker, transactions.Min(t => t.Date), transactions.Max(t => t.Date));

            var dailyYieldChanges = CalculateDailyYieldChanges(transactions, stockPrices);

            return dailyYieldChanges;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error calculating daily yield changes: {ex.Message}");
        }
    }

    private List<DailyYieldChangeDto> CalculateDailyYieldChanges(List<TransactionResponseDto> transactions, List<Stock> stockPrices)
    {
        var dailyYieldChanges = new List<DailyYieldChangeDto>();

        foreach (var transaction in transactions)
        {
            var stockPrice = stockPrices.FirstOrDefault(sp => sp.Date == transaction.Date);

            if (stockPrice != null)
            {
                var yieldChange = new DailyYieldChangeDto
                {
                    Date = transaction.Date,
                    StockTicker = transaction.StockTicker,
                    TransactionType = transaction.TransactionType,
                    Quantity = transaction.Quantity,
                    PurchasePrice = (decimal)(stockPrice.OpenPrice ?? 0),
                    CurrentPrice = stockPrice.ClosestPrice ?? 0
                };

                dailyYieldChanges.Add(yieldChange);
            }
        }

        return dailyYieldChanges;
    }

   
}