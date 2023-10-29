using Accounts.Domain.Enums;

namespace Accounts.Domain.Abstraction.Services
{
    public interface ICurrencyConverterService
    {
        decimal Convert(CurrencyCode currentCurrency, CurrencyCode wantedCurrency, decimal sum);
    }
}