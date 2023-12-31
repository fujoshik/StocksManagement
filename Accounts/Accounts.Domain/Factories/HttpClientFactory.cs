﻿using Accounts.Domain.Abstraction.Clients;
using IHttpClientFactory = Accounts.Domain.Abstraction.Factories.IHttpClientFactory;

namespace Accounts.Domain.Factories
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly ISettlementClient _settlementClient;
        private readonly IStockApiClient _stockApiClient;

        public HttpClientFactory(IStockApiClient stockApiClient,
                                 ISettlementClient settlementClient)
        {
            _settlementClient = settlementClient;
            _stockApiClient = stockApiClient;
        }
        public IStockApiClient StockApiClient()
        {
            return _stockApiClient;
        }

        public ISettlementClient SettlementClient()
        {
            return _settlementClient;
        }
    }
}
