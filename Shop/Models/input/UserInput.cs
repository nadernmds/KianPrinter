using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models.ApiEntity;

namespace Shop.Models
{
    public class UserInput
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string mobile { get; set; }
        public int? usergroupID { get; set; }
        public string email { get; set; }
        public bool sex { get; set; }
    }
}