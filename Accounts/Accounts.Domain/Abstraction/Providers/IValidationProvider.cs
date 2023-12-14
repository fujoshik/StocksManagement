namespace Accounts.Domain.Abstraction.Providers
{
    public interface IValidationProvider
    {
        void TryValidate<TDto>(TDto dto);
        Task TryValidateAsync<TDto>(TDto dto);
    }
}
