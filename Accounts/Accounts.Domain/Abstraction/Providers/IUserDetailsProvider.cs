namespace Accounts.Domain.Abstraction.Providers
{
    public interface IUserDetailsProvider
    {
        Guid GetAccountId();
    }
}
