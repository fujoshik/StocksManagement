using Accounts.Domain.Abstraction.Providers;
using Accounts.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using ValidationException = Accounts.Domain.Exceptions.ValidationException;

namespace Accounts.Domain.Providers
{
    public class ValidationProvider : IValidationProvider
    {
        private IValidatorFactory _validatorFactory;

        public ValidationProvider(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public void TryValidate<TDto>(TDto dto)
        {
            var result = TryGetValidator<TDto>().Validate(dto);

            ThrowIfInvalid(result);
        }

        public async Task TryValidateAsync<TDto>(TDto dto)
        {
            var result = await TryGetValidator<TDto>().ValidateAsync(dto);

            ThrowIfInvalid(result);
        }

        private void ThrowIfInvalid(ValidationResult result)
        {
            if (!result.IsValid)
            {
                var message = string.Join(";  ", result.Errors.Select(x => x.ErrorMessage));
                throw new ValidationException(message);
            }
        }

        private IValidator<TDto> TryGetValidator<TDto>()
        {
            var dtoTypeValidator = _validatorFactory.GetValidator<TDto>();
            if (dtoTypeValidator is null)
            {
                throw new NoExistingValidatorForGivenTypeException(typeof(TDto));
            }
            return dtoTypeValidator;
        }
    }
}
