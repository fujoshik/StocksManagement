using Settlement.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Wallet;
using Newtonsoft.Json;
using StockAPI.Infrastructure.Models;
using Settlement.Domain.Abstraction.Routes;
using System.Net;
using Settlement.Domain.DTOs.Transaction;
using Settlement.Domain.Abstraction.Repository;
using Settlement.Domain.DTOs.Settlement;

namespace Settlement.Domain.Services
{
    public class ConnectionService : IHttpClientService
    {
        private readonly HttpClient httpClient;
        private readonly IWalletRoutes walletRoutes;
        private readonly IStockRoutes stockRoutes;
        private readonly ISettlementRepository settlementRepository;

        public ConnectionService(HttpClient httpClient, IWalletRoutes walletRoutes,
            IStockRoutes stockRoutes, ISettlementRepository settlementRepository)
        {
            this.httpClient = httpClient;
            this.walletRoutes = walletRoutes;
            this.stockRoutes = stockRoutes;
            this.settlementRepository = settlementRepository;
        }

        public async Task<WalletResponseDto> GetWalletBalance(Guid walletId, TransactionRequestDto transaction)
        {
            HttpResponseMessage response = await httpClient.GetAsync(walletRoutes.Routes["GET"].Replace("{id}", walletId.ToString()));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string data = await response.Content.ReadAsStringAsync();
                WalletResponseDto wallet = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                return await Task.FromResult(wallet);
            }
            else
            {
                await settlementRepository.InsertIntoFailedTransaction(transaction);
                return new WalletResponseDto();
            }
        }

        public async Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
        {
            HttpResponseMessage response = await httpClient.GetAsync(stockRoutes.Routes["GET"]
                .Replace("{date}", date)
                .Replace("{stockTicker}", stockTicker));

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                Stock stock = JsonConvert.DeserializeObject<Stock>(data);
                return await Task.FromResult(stock);
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
    }
}
