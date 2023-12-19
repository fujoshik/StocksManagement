using Accounts.Domain.DTOs.Wallet;
using FluentValidation;

namespace Accounts.Domain.Validators.Wallet
{
    public class DepositDtoValidator : AbstractValidator<DepositDto>
    {
        public DepositDtoValidator()
        {
            RuleFor(x => x.Sum).GreaterThanOrEqualTo(0);

            RuleFor(x => x.CurrencyCode).IsInEnum();
        }
    }
}
