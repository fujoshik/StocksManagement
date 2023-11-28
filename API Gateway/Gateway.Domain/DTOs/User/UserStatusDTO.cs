using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.User
{
    public class UserStatusDTO
    {
        public string UserId { get; set; }
        public UserType CurrentStatus { get; set; }
        public DateTime LastStatusChange { get; set; }
        
    }

}
