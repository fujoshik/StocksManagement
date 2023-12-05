using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.Authentication
{
    public class RegisterTrialDTO
    {
        public string Email { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
