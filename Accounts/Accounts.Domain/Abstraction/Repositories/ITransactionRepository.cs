namespace Accounts.Domain.Abstraction.Repositories
{
    public interface ITransactionRepository : IBaseRepository
    {
        Task DeleteByAccountIdAsync(Guid accountId);
    }
}
