using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.DTOs.Account
{
    public class UserAccountInfoDto
    {
        public Guid AccountId { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
