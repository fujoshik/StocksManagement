﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.DTOs.Account
{
    public class UserAccountInfoDto : BaseDto
    {
        public decimal InitialBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
