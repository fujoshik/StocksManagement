using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.DTOs.User
{
    public class UserProfileDto
    {
        public string UserId { get; set; }
        public UserType UserType { get; set; }
        public int LoadedAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
