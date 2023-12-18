using Gateway.Domain.DTOs.Stock;

namespace Gateway.Domain.Abstraction.Clients.AccountClient
{
    public interface IAccountClient
    {
        IAuthenticateAccountClient GetAuthenticateAccountClient();
        IWalletAccountClient GetWalletAccountClient();
        IUserAccountClient GetUserAccountClient();
        IStockAccountClient GetStockAccountClient();
    }
}
