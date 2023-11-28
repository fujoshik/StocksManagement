namespace Accounts.Domain.Abstraction.Services
{
    public interface IDeleteWhenTrialEndsService
    {
        Task<bool> DeleteAccountWhenTrialEndsAsync(Guid accountId, string dateToDelete);
    }
}
