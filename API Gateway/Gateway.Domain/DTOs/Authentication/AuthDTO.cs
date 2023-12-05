using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.Authentication
{
    public record AuthDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
