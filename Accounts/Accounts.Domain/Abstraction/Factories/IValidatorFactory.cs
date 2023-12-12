using FluentValidation;

namespace Accounts.Domain.Abstraction.Factories
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
    }
}
