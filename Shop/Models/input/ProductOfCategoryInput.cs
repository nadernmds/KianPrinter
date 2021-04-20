using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models.ApiEntity;

namespace Shop.Models
{
    public class ProductOfCategoryInput
    {
        public int? catID { get; set; }
        public string sort { get; set; }
        public int page { get; set; }
    }
}