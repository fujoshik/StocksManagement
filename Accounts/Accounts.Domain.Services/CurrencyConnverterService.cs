using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.Constants;
using Accounts.Domain.Enums;
using System.Reflection;

namespace Accounts.Domain.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private List<FieldInfo> GetConstants()
        {
            var fieldInfos = typeof(CurrencyConstants).GetFields(BindingFlags.Public |
                BindingFlags.Static | BindingFlags.FlattenHierarchy);

            return fieldInfos.Where(fi => fi.IsInitOnly).ToList();
        }

        public decimal Convert(CurrencyCode currencyTo, CurrencyCode currencyFrom, decimal sum)
        {
            if (currencyFrom != currencyTo)
            {
                decimal value = 0;

                var constantName = GetConstants()
                    .Select(x => x.Name)
                    .FirstOrDefault(x => x.StartsWith(currencyFrom.ToString()) && x.EndsWith(currencyTo.ToString()));

                if (constantName != null)
                {
                    value = (decimal)GetConstants()
                        .FirstOrDefault(x => x.Name == constantName)
                        .GetValue(new CurrencyConstants());
                }
                
                return sum * value;
            }

            return sum;
        }
    }
}
