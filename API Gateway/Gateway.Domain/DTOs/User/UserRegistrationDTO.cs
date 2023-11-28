using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.User
{
    public class UserRegistrationDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public decimal InitialBalance { get; set; }
        
    }

}
