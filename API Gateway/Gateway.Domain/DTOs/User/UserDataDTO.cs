using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.User
{
    public class UserDataDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public decimal AccountBalance { get; set; }
        public UserType UserType { get; set; }
        
    }

}
