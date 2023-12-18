using Settlement.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Wallet;
using Newtonsoft.Json;
using StockAPI.Infrastructure.Models;
using System.Net;
using Settlement.Domain.DTOs.Transaction;

namespace Settlement.Domain.Services
{
    public class ConnectionService : IHttpClientService
    {
        private readonly IConnectionDependencies dependencies;
        public ConnectionService(IConnectionDependencies dependencies)
        {
            this.dependencies = dependencies;
        }

        public async Task<WalletResponseDto> GetWalletBalance(Guid walletId, TransactionRequestDto transaction)
        {
            HttpResponseMessage response = await dependencies.Http.GetAsync(dependencies.Wallet.Routes["GET"].Replace("{id}", walletId.ToString()));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string data = await response.Content.ReadAsStringAsync();
                WalletResponseDto wallet = JsonConvert.DeserializeObject<WalletResponseDto>(data);
                return await Task.FromResult(wallet);
            }
            else
            {
                await dependencies.Repository.InsertIntoFailedTransaction(transaction);
                return new WalletResponseDto();
            }
        }

        public async Task<Stock> GetStockByDateAndTicker(string date, string stockTicker)
        {
            HttpResponseMessage response = await dependencies.Http.GetAsync(dependencies.Stock.Routes["GET"]
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
