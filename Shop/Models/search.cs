using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop.Models
{
    public class search
    {

        public List<details> category { get; set; }
        public List<details> brand { get; set; }
        public List<details> product { get; set; }
    }

    public class details
    {
        public int itemID { get; set; }
        public string name { get; set; }
    }

}