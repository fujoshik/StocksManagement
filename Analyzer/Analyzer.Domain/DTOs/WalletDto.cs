using Accounts.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Domain.DTOs
{
    public class WalletDto
    {
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public CurrencyCode CurrencyCode { get; set; }

        public UserDataDto? UserData { get; set; }
    }
}
