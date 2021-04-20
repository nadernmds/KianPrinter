using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models.ApiEntity;

namespace Shop.Models
{
    public class PasswordInput
    {
        public string username { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
    }
}