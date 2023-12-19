using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.DTOs.Stock;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients.AccountClient
{
    public class AccountClient : IAccountClient
    {
        private readonly IAuthenticateAccountClient _authenticateAccountClient;
        private readonly IWalletAccountClient _walletAccountClient;
        private readonly IUserAccountClient _userAccountClient;
        private readonly IStockAccountClient _stockAccountClient;

        public AccountClient(IAuthenticateAccountClient authenticateAccountClient,
                             IWalletAccountClient walletAccountClient,
                             IUserAccountClient userAccountClient,
                             IStockAccountClient stockAccountClient)
        {
            _authenticateAccountClient = authenticateAccountClient;
            _walletAccountClient = walletAccountClient;
            _userAccountClient = userAccountClient;
            _stockAccountClient = stockAccountClient;
        }

        public IAuthenticateAccountClient GetAuthenticateAccountClient()
        {
            return _authenticateAccountClient;
        }

        public IWalletAccountClient GetWalletAccountClient()
        {
            return _walletAccountClient;
        }

        public IUserAccountClient GetUserAccountClient()
        {
            return _userAccountClient;
        }

        public IStockAccountClient GetStockAccountClient()
        {
            return _stockAccountClient;
        }
    }
}