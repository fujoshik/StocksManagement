﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settlement.Domain.DTOs.Settlement
{
    public class SettlementResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public decimal StockPrice { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
