using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Buildings.Model
{
    public class Purchase
    {
        public int item_id { get; set; }
        public int item_category_id { get; set; }
        public double cost { get; set; }
    }
}