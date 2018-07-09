using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buildings.Model
{
    public class SessionInfo
    {
        public int building_id { get; set; }
        public List<Purchase> purchases { get; set; }
    }
}