using Analyzer.API.Analyzer.Domain.Abstracions.Interfaces;

namespace Analyzer.API.Analyzer.Domain.Abstracions.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient accountClient;
        private readonly HttpClient settlementClient;

        public HttpClientService()
        {
            accountClient = new HttpClient();
            accountClient.BaseAddress = new Uri("https://localhost:7073/accounts-api/wallets/");

            settlementClient = new HttpClient();
            settlementClient.BaseAddress = new Uri("http://localhost:5064");
        }

        public HttpClient GetAccountClient()
        {
            return accountClient;
        }

        public HttpClient GetSettlementClient()
        {
            return settlementClient;
        }
    }
}
