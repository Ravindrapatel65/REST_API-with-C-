using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buildings.Model
{
    public class AnalyticsData
    {
        public string manufacturer { get; set; }
        public string market_name { get; set; }
        public string codename { get; set; }
        public string model { get; set; }
        public UsageStatistics usage_statistics { get; set; }
    }
}