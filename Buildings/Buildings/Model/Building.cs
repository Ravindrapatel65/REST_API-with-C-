using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buildings.Model
{
    public class Building
    {
        public int building_id { get; set; }
        public string building_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
    }
}