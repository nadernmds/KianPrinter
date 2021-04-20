using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class Chart
    {
            public int productID { get; set; }
            public List<string> xrows { get; set; }
            public List<int> yrows { get; set; }
    }
}