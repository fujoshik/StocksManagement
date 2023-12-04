using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway.Domain.Settings
{
    public class AccountSettings
    {
        public string RegisterTrialRoute { get; set; }
        public string RegisterRoute { get; set; }
        public string LoginRoute { get; set; }
    }
}
