using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using Accounts.Domain.Enums;
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
            // Получаваме транзакциите за дадения акционер и символ на акцията
            var transactions = await httpClientService.GetTransactions(accountId, stockTicker);


            // Получаваме цените на акцията за всеки ден в рамките на периода на транзакциите
            var stockPrices = await httpClientService.GetStock(stockTicker, transactions.Min(t => t.Date), transactions.Max(t => t.Date));

            // Изчисляваме промените в дохода за всеки ден
            var dailyYieldChanges = CalculateDailyYieldChanges(transactions, stockPrices);

            return dailyYieldChanges;
        }
        catch (Exception ex)
        {
            // Обработка на грешки
            throw new InvalidOperationException($"Error calculating daily yield changes: {ex.Message}");
        }
    }

    // Преименувайте метода тук
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