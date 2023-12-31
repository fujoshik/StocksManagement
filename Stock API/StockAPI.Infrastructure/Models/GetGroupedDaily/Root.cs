﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAPI.Infrastructure.Models.GetGroupedDaily
{
    public class Root
    {
        public int? queryCount { get; set; }
        public int? resultsCount { get; set; }
        public bool? adjusted { get; set; }
        public List<Result>? results { get; set; }
        public string? status { get; set; }
        public string? request_id { get; set; }
        public int? count { get; set; }
    }
}
