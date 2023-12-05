using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Abstraction.Services
{
    public interface IBlacklistService
    {
        public bool IsEmailBlacklisted(string email);
        bool IsUserBlacklisted(string userId);
    }

}
