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
        private readonly IService service;

        public CalculateCurrentYieldService(
            IHttpClientService httpClientService,
            IDailyYieldChanges dailyYieldChangesService,
            IPercentageChange percentageChangeService,
            IService service)
        {
            this.httpClientService = httpClientService;
            this.dailyYieldChangesService = dailyYieldChangesService;
            this.percentageChangeService = percentageChangeService;
            this.service = service;
        }

        public async Task<TransactionResponseDto> CalculateCurrentYield(Guid accountId, string stockTicker, string data)
        {
            try
            {
                var settlementDto = await httpClientService.GetTransactions(accountId,stockTicker);

                if (settlementDto?.Quantity != null)
                {
                    var stockData = await httpClientService.GetStockData(stockTicker, data);

                    if (stockData != null)
                    {
                        var closePrice = stockData.ClosestPrice.Value;
                        var quantity = settlementDto.Quantity;
                        var openPrice = stockData.OpenPrice.Value;

                        var currentYield = (((decimal)quantity * (decimal)openPrice) - closePrice);

                        var transactionData = new TransactionResponseDto
                        {
                            WalletId = accountId,
                            StockTicker = stockTicker,
                            Date = DateTime.Now.ToString(),
                            Quantity = quantity,
                            // Add other properties as needed
                        };

                        return transactionData;
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
                // Example of fetching transactions from an external API
                var apiUrl = $"https://localhost:7073/accounts-api/transactions/get-transactions";

                var response = await httpClientService.GetAsync(apiUrl);


                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    // Deserialize the content into a List<TransactionResponseDto> using JSON deserialization
                    var transactions = JsonConvert.DeserializeObject<List<TransactionResponseDto>>(content);
                    return transactions;
                }
                else
                {
                    // Handle the case where the API request was not successful
                    throw new HttpRequestException($"Failed to retrieve transactions. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the process
                throw new InvalidOperationException($"Error retrieving transactions: {ex.Message}", ex);
            }
        }
    





    //public async Task<decimal> PercentageChange(Guid userId, string stockTicker, string data)
    //{
    //    return await percentageChangeService.PercentageChange(userId, stockTicker, data);
    //}

    public bool IsValidMarketPrice(decimal marketPrice)
        {
            return marketPrice > 0;
        }
    }
}
