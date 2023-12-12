using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Infrastructure.Entities
{
    public class User : BaseEntity

    {
        public int Id { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        
    }
   
}
