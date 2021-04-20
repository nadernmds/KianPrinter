using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class customProduct
    {
        public int? productID { get; set; }
        public Product product { get; set; }
        public int? colorID { get; set; }
        public int? count { get; set; }
        public string colorName { get; set; }
        public string productName { get; set; }

        public Invoice invoice { get; set; }
    }
}