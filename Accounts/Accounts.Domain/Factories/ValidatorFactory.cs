using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using IValidatorFactory = Accounts.Domain.Abstraction.Factories.IValidatorFactory;

namespace Accounts.Domain.Factories
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return _serviceProvider.GetService<IValidator<T>>();
        }
    }
}
