using Accounts.Domain.Clients;
using Analyzer.Domain.Abstracions.Interfaces;
using Analyzer.Domain.DTOs;
using Newtonsoft.Json;


namespace Analyzer.API.Analyzer.Domain.Services
{
    public class CalculateCurrentYieldService : ICalculationService
    {
        private readonly IHttpClientService httpClientService;
        private readonly IDailyYieldChanges dailyYieldChangesService;
        private readonly IPercentageChange percentageChangeService;
        

        public CalculateCurrentYieldService(
            IHttpClientService httpClientService,
            IDailyYieldChanges dailyYieldChangesService,
            IPercentageChange percentageChangeService
            )
        {
            this.httpClientService = httpClientService;
            this.dailyYieldChangesService = dailyYieldChangesService;
            this.percentageChangeService = percentageChangeService;
           ;
        }

        public async Task<decimal> CalculateCurrentYield(Guid accountId, string stockTicker, string data)
        {
            try
            {
                var transactionDataList = await httpClientService.GetTransactions(accountId, stockTicker);

                if (transactionDataList != null && transactionDataList.Any())
                {
                    var totalQuantity = transactionDataList.Sum(t => t.Quantity);

                    var stockData = await httpClientService.GetStockData(stockTicker, data);

                    if (stockData != null)
                    {
                        var closePrice = stockData.ClosestPrice.Value;
                        var openPrice = stockData.OpenPrice.Value;

                        var currentYield = (((decimal)totalQuantity * (decimal)openPrice) - closePrice);

                        return currentYield;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to calculate current yield. Stock data not available.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Unable to calculate current yield. Transaction data not available.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error calculating current yield: {ex.Message}");
            }
        }

        public async Task<List<TransactionResponseDto>> GetTransactionsByAccountIdTickerAndDateAsync(Guid accountId, string ticker, DateTime dateTime)
        {
            try
            {
                var apiUrl = $"https://localhost:7073/accounts-api/transactions/get-transactions";

                var response = await httpClientService.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var transactions = JsonConvert.DeserializeObject<List<TransactionResponseDto>>(content);
                    return transactions;
                }
                else
                {
                    throw new HttpRequestException($"Failed to retrieve transactions. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving transactions: {ex.Message}", ex);
            }
        }

    public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }
    }
}
