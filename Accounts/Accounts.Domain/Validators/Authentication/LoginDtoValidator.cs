using Accounts.Domain.DTOs.Authentication;
using FluentValidation;

namespace Accounts.Domain.Validators.Authentication
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty();

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
