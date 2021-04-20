using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shop.Models.ApiEntity;

namespace Shop.Models.input
{
    public class AddressInput
    {
        public int addressID { get; set; }
        public string addressDetail { get; set; }
        public string postalCode { get; set; }
        public int? userID { get; set; }
        public int? cityID { get; set; }
    }
}