using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class filter
    {
        public long nameID { get; set; }
        public string name { get; set; }
        public List<ProductAttribute> productAttribute { get; set; }
    }
}